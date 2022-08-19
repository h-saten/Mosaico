# updating semvar values:
you have to edit github secret : MAJOR/MINOR/PATCH (Patch is automatic, every push increments its value)

# ISSUES WITH SELF-HOSTED
Basically, when doing self-hosted on upload to azure storage, there were many problems - mainly with DIND (docker in docker), the container running on cluster, couldnt start
another docker instance (binding to socket error, cannot create new container), docker service couldnt run properly.
To fix it you need to properly implement new base image which supports docker in docker
# PIPELINE YAMLS:
There are 5 main Pipeline YAMLS (as of writing this - 31.03.2022):

- build-backend.yml: \
Launches after `PR`, on Push to `/src/` dir \
uses manifest: `./.github/workflows/build_backend`

- build-frontend.yml: \
Launches after `PR`, on Push to `'/frontend/mosaico-web-ui/'` dir \
uses manifest: `./.github/workflows/build-frontend.yml` 

- builf-id-frontend.yml: \
Launches after `PR`, on Push to `'frontend/mosaico-id-ui/'` dir \
uses manifest: `./.github/workflows/build-id-frontend.yml`

- cd-frontend-backend.yml: \
Launches after Push to: `/src/` `frontend/mosaico-web-ui/`,`frontend/mosaico-id-ui/`,`.github/`,`infrastructure/` directories
Main manifest used is: `./github/workflows/cd-frontend-backend.yaml` \
(Other manifests described below)

- ci-smart-contracts.yml: \
Launches after PR to `smart_contracts/` dir 
Main manifest used is: `./github/workflows/ci-smart-contracts.yaml` \
(Other manifests described below)

- cd-hotfix.yaml: \
Launches after Push to: `/src/` `frontend/mosaico-web-ui/`,`frontend/mosaico-id-ui/`,`.github/`,`infrastructure/` directories
`ON PUSH TO BRANCH: 'hotfix/**'`
Main manifest used is: `./github/workflows/cd-frontend-backend.yaml` \
(Same manifests as cd-frontend-backend.yaml - CI/CD, but without the dev env)

# cd-frontend-backend manifests:
## increment_version
- runs on: `ubuntu-latest`
- environment: `NONE`
- used actions: `mathieudutour/github-tag-action@v6.0` | `increment-semantic-version@1.0.2` | `release-action@v1` | `actions-set-secret@v2.0.0`
The very first job that runs on pipeline is `incerment_version`, it increments the patch part of semantic versioning (MAJOR.MINOR.PATCH)
Sometimes it is buggy and fails to increment version, or tag a new release. Reason for that is launching quickly one pipeline after another (~30/45s)
When that Error occurs, you may have to manually increment a github secret: `APP_VERSION` by safe 3 values. (example: pipeline failed at 2.0.100, you increment it to 2.0.103)

## backend
- runs on: `ubuntu-latest`
- environment: `NONE`
- used manifests: `actions/backend_core` 
This job is very similliar to identity, in a way, that the steps used are almost the same. We set up Github Hosted Runner environment, stting up java,dotnet,caching sonarcloud, launching scan, then building dotnet, publishing it, building docker image, scanning it with Trivy and finally pushing it to ACR (Azure Container Registry).

## frontend
- runs on: `ubuntu-latest`
- environment: `NONE`
- used manifests: `actions/build_truffle` | `actions/frontend`
In this Job, most significant difference is using `build_truffle` action. \
First we set up Node, Install Truffle, Install Truffle dependencies, and we run `truffle compile` (Here, we aren't running any tests for truffle!).
After that step, we move to `actions/frontend`. This is very simple action, that sets up Github Runner env, builds npm and runs sonar cloud against it (Static Code Analysis). Although we run npm run build with `--configuration` Argument, this action fails to run without this argument, it might change later.

## mosaico_cli
- runs on: `ubuntu-latest`
- environment: `NONE`
- used manifests: `actions/backend_cli`
This job is very similliar to identity, in a way, that the steps used are almost the same. We set up Github Hosted Runner environment, stting up java,dotnet,caching sonarcloud, launching scan, then building dotnet, publishing it, building docker image, scanning it with Trivy and finally pushing it to ACR (Azure Container Registry).

## identity
- runs on: `ubuntu-latest`
- environment: `NONE`
- used manifests: `frontend_identity` | `backend_identity` 

First frontend of identity is built, using angular version `13.1.1` (ng build --config $ENV `dev/test/production`)\
We also launch Sonarcloud scanner. After Frontend step is finished, we move on to Building Backend of Identity. \
Commands used are, sonarcloud cache, start scanning, dotnet build / publish, and when it is published to `OUTPUT_FOLDER`, we proceed to build docker image, scan it, and when there are no serious vulnerabilities (classified by Trivy as High or Critical), we push 2 tags of image; One with `APP_VERSION` Tag, and one with `:latest` Tag. \

## backup-core-id-sql-(dev/test/prod)
- runs on: `ubuntu-latest`
- environment: `dev/test/prod`
- used manifests: `backup_sql_databases` \
First, log in to azure using az cli, next use action `nanzm/get-time-action@v1.1` to set up 2 variables for Time. One is `Current time`, the other is `Future time`. Main reason for that, is to create SAS token to gain access to blob storage for `1 Hour`. When creating backup, we create 2 instances of it. One for `CORE`, the other for `IDENTITY`. They have corresponding prefix; `CORE` and `IDENTITY`. The `.bacpac` file name which contains backup for CORE/IDENTITY database is constructed such as: 
`IDENTITY`/`CORE`_`Current time`(Value from action)_`APP_VERSION`.bacpac.
Example bacpac file should look something like this: `CORE_20220222120434_2.0.334.bacpac`.
For ease of making restore of backup happen from latest available backup, we create only 2 additional instances of backup named `IDENTITY_LATEST` and `CORE_LATEST`.
They are deleted and recreated everytime this step of pipeline is ran, so we have always available two latest instances of backup from core and ID.

## build_upload_deploy_aks_(dev/test/prod)
- runs on: [self-hosted,dev/test/prod] (dev/test/prod is a tag attached to self hosted runner that runs on our AKS)
- environment" `dev/test/prod`
- used manifests: `actions/frontend_config` | `actions/upload_storage_core_2.33.1` - for DEV | `actions/upload_storage_core_2.34.1` | `actions/run_mosaico_cli` | `actions/aks_deployment`

### Frontend config:
- very similliar to action/frontend, set up env, run sonar scanner, npm install dependencies, ng build --config dev/test/prod --output-path, and then run `actions/upload-artifact@v2`

### actions/upload_storage_core_2.33.1:
- There are 2 versions of this composite action; one for AZ CLI v2.33.1 and v2.34.1. Reason for that, is for unknown reasons, self hosted runner on DEV has AZ cli 2.34.1 which requires to use `--overwrite` argument to run properly `az storage blob upload-batch`. In this action with version 2.33.1 we run `az storage blob upload-batch` without --overwrite argument, because it is not necessary. Before running az storage blob upload batch, we have to log-in to az, using `az login` command with proper arugments.

### actions/run_mosaico_cli:
- This action is used to migrate Database CORE and IDENTITY. (separately, one run for ID, next run for CORE)
First, the kubectl is set up on runner. Next, the action `bluwy/substitute-string-action@v1` is used to swap credential in appsetings.json file, to contain new Key Vault Client Secret. After that, authorization to AKS is ran, using `AZURE_CREDENTIALS` Github Secret. Moving forward, the configmap from app settings folder is created, and finally we delete any existant left-over jobs, and then we apply the manifest located in `infrastructure/kubernetes_yamls/[dev/test/prod]/jobs/cli_migrate_database_[core/identity].yaml`

### actions/aks_deployment:
- This action is used to deploy proper manifest.
Kubectl is set up, `bluwy/substitute-string-action@v1` is used to swap credential in appsettings, next authorization to k8s is done, namespace is created, configmap is created, after that we deploy proper manifest file (they are located in `infrastructure/kubernetes_yamls/dev/core/backend.yaml` for example). Last part of that action
is to run kubectl rollout restart.

# SQL Backup
To backup SQL database, we use azure cli command az sql db export, which then uses SAS key to authenticate to azure storage account which will hold all backups.
Make sure that connection string in line, where we create SAS key is current one and appropiate for the storage account that you use to hold backups.


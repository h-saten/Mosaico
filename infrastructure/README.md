# Terraform
### Installation
---
- Install terraform from: *https://www.terraform.io/downloads.html* (selecting appropiate pre compiled library). Then add it to system PATH.
- Or use *https://chocolatey.org/* and then run `choco install terraform`
---
### Using Terraform
---
- *When working on new module, you can use `create_dir_struct.py` to create basic Tree of directories needed for said module.*
---
- To deploy infrastructure for specific enviroment: [`dev/test/prod`] you have to:
- Set up `dev/test/prod`.tfvars files, which contain:
```
    subscription_id -> microsoft sponsorship sub id
    tenant_id       -> azure AD tenant id
    client_id       -> service principal of dev
    client_secret   -> own secret created in serv. principal
    network_prefix -> used to set proper virtual network prefix, eg: for dev we use prefix X, so deployed network address looks like : 1.`X`.1.1
    shared_tenant_id,shared_client_id,shared_client_secret -> same as tenant_id,client_id,client_secret
    shared_subscription_id -> subscription id from which we get shared vnet-vpn-hub to peer
    shared_rg_name -> rg name of resource group where vpnhub exists
    shared_vnet_network_name -> name of network used for peering.
    serice_principal_id -> used to assign role for key vault
    dev_team_id -> used to assign role for key vault
    location -> West Europe
    env -> for dev enviroment use dev, for test: test, for prod: prod
    sqlserver_password -> password of sql server

  ```
- Then if proper .tfvars exist, run following command to deploy:
```
    * *dev*: `terraform apply -var-file="./dev.tfvars"`
    * *test*: `terraform apply -var-file="./test.tfvars"`
    * *prod*: `terraform apply -var-file="./prod.tfvars"`
```
- If you wish to destroy deployed infrastructure in specified enviroment, run:
```
    * *dev*: `terraform destroy -var-file="./dev.tfvars"`
    * *test*: `terraform destroy -var-file="./test.tfvars"`
    * *prod*: `terraform destroy -var-file="./prod.tfvars"`
```

- When configuring Storage Accounts, to set another subnet, that DNS works with, provide another `vnet_subnet_id` (default one is aks_virtual_nodes)
---
### Using MSSQL
- If you want to use MSSQL, download SSMS (`https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15`), Connect to our VPN using `Azure VPN client`, add local entry in hostfile (`C:\Windows\System32\drivers\etc\hosts` *entry*:&nbsp;&nbsp;`x.x.x.x`&nbsp;&nbsp;&nbsp;&nbsp;`x.database.windows.net`) then select: Database Engine, provide proper server name, and use authentication: `Azure Active Directory - Universal with MFA` , as username provide your account which is in `Azure AD`.
### Terratests
Terratest (https://terratest.gruntwork.io/) open source engine to run terraform tests using Go language. To setup and run tests, follow the instruction:
```
- install Go https://go.dev/dl/
- Open Terratests folder and run `go mod donwload` and then `go get -u -v -f all`. This will download all dependencies (packages from network like npm)
- Make sure your tests have terraform configured and they point to the right directory, where main module is stored
- If you want to run single file, execute command `go test -v -run ./virtual-network/dev_test.go` or just `go test ./virtual-network`
```
Remember to check your import dependencies. They should be installed and listed in `go.sum` file (like `package-lock.json`). You can download all dependencies for the package using `go get mosaico_terratests/virtual-network` where `mosaico_terratests` is a module name and `virtual-network` is a folder with go files.

Following repository contains a lot of useful examples on how to write tests https://github.com/gruntwork-io/terratest/tree/master/test/azure

# Secrets

Most important and sensitive secrets that should be stored on Key Vault and privately injected into the solution:
(Some of them are lifecycle ignored by terraform)
Also, in .tf file which reflects key vault, there are some "weird" resources at bottom half of the file; they are used to ensure of proper read, and not overwriting some Keys, because they are "newer" to terraform.
It is done so to avoid terraform apply failure, which happened.

- `SqlServer--ConnectionString` - Connection string to the main SQL databse
- `Cache--RedisConnectionString` - Redis Connection string for Cache
- `EventSourcing--RedisConnectionString` - Redis Connection String for Event Sourcing
- `ServiceBus--ConnectionString` - connection string to the Azure Service Bus
- `AzureBlobStorage--ConnectionString` - Azure Storage Account connection string
- `AzureBlobStorage--EndpointUrl` - URL of the Azure Storage account, based on which URLs will be generated
- `Ethereum--AdminAccount--PrivateKey` -Private key of the administrative account, which will perform blockchain operations for custodial wallets
- `CoreService--ResourceUrl` - URL of the Backend
- `Moralis--ApiKey` - API key to access Moralis SDK
- `Hangfire--ConnectionString` - SQL Server connection string where Hangfire data will be stored
- `EmailLabs--AppKey` - Email Labs App key
- `EmailLabs--SecretKey` - Email Labs Secret key
- `SmsLabs--AppKey` - Sms labs app key
- `SmsLabs--SecretKey` - Sms labs secret key
- `TokenizerSqlServer--ConnectionString` - Tokenizer sql server conn string
- `Loggers--ApplicationInsightsLogger--InstrumentationKey` - Loggers App insights Instrumentation key
- `IdentitySqlServer--ConnectionString` - Identity SQL server connection string
- `ServiceBus--ConnectionString` - connection string for servicebus
# Service Bus 

## Topics

- `users` - User Management Module (Identity)
- `wallets` - Crypto Wallet Module
- `projects` - Project (Crowdsale) Module
- `nfts` - NFT Module (Marketplace and Investment packages)
- `documents` - Document management module
- `companies` - Business management module

## Queues

-

## Subscriptions

For each of topic and queue, we have to create following subscriptions:

- `api` - Mosaico Backend (REST API)

# Github Actions
- Setting up secrets:
- To have proper secrets for terraform workflow you need following secrets:
  * AZURE_CREDENTIALS        -> `combined secrets (https://github.com/marketplace/actions/azure-kubernetes-set-context)`
  * ACTIONS_STEP_DEBUG       -> `Used to set up debug output`
  * AZURE_STORAGE_CONNECTION_STRING -> `Connection string to azure storage main one stmosaico`
  * AZURE_STORAGE_CONTAINER_NAME    -> `Name of storage container name for frontend files ^this one`
  * CORE_SQL_DB_NAME                -> `database name of sql database for core`
  * IDENTITY_SQL_DB_NAME            -> `database name of sql database for identity
  * KEY_VAULT_CLIENT_SECRET         ->
  * RESOURCE_GROUP                    -> `Resource group for whole env on Azure`
  * SQL_ADMIN_LOGIN                   -> `Sql admin login for SQL server`
  * SQL_ADMIN_PASSWORD                -> `Password for that User`
  * SQL_SERVER_NAME                   -> `Name of the main SQL server`
  * STORAGE_ACCOUNT_NAME              -> `Name of the main mosaico storage account on enviroment resource group (dev/test/prod)`
  * STORAGE_CONTAINER_NAME_FOR_BACKUP -> `Name of Container which holds SQL backups`
  * STORAGE_CONTAINER_URI             -> `URI of storage container holding SQL backups (the main URI, which ends without slash, example: https://[STORAGE_ACCOUNT_NAME].blob.core.windows.net)`
  * RESOURCE_GROUP_TERRAFORM          -> `dedicated resource group name which holds terraform storage account`
  * SONAR_BACKEND_SECRET              -> `Sonarcloud token from CREATE Github secret tab which pops up after analyzing new project`
  * SONARCLOUD_GITHUB_TOKEN           -> `REMOVED`
  * TERRAFORM_STORAGE_CONNECTION_STRING -> `Connection string to storage account holding terraform state`
# Kubernetes 
- `(when setting up role assignments for kubernetes vmss on azure, i assign it to all vmss because it is not possible to get specific VMSS name https://github.com/hashicorp/terraform-provider-azurerm/issues/6217)`
- Currently, to properly use kubernetes, provide Kubernetes public IP, and its DNS FQDN, to local host file.
- provide address from: VNET-MOSAICO-{env}> -> `NETWORK INTERFACE OF KUBE-APISERVER.NIC..` 
- So your host file should look like: 
<pre>
- `IP FROM NETWORK INTERFACE`     `AKS-MOSAICO-<ENV> API server address`
</pre>
- or just use update-host.ps1 which is inside infrastructure directly - it is up to you (make sure the values in script are proper and renewed for each enviroment of cluster; dev/test/prod).
- to launch cluster from scratch, use script inside /infrastructure/kubernetes_yamls directory, it will create Azure AD for pods, NMI/MIC and set up baseline for ingress to be created later in the pipeline.
## Alternatively just run /infrastructure/update-hosts.ps1 to do that
---
- Installing kubernetes-cli (kubectl) on windows:
- Using chocolatey:
`choco install kubernetes-cli`
---
- When working with kubernetes, it is advised to use this IDE: `https://k8slens.dev/`
---
- Installing HELM-CLI: 
- Using chocolatey: `choco install kubernetes-helm`
---
- To ease switching between clusters aks dev/test/prod it is recommended to use kubens with kubectx (choco install kubens kubectx)
- More information available here: `https://github.com/ahmetb/kubectx#windows-installation-using-chocolatey`


# `All yamls used below are located in /infrastructure/kubernetes_yamls`

# Application gateway

```
  App gateway is currently configured to work in line with AGIC - Azure Gateway Ingress Controller
  It works on our cluster, and needs additional managed identities permissions - Contributor/Reader to work, Managed identity Operator, etc. so AGIC can manage it.

  if you get some errors during clean cluster install, verify if proper Managed ID is provided to helm chart helm_config.yaml

  Followed configuration, as described on : `https://github.com/Azure/application-gateway-kubernetes-ingress/blob/master/docs/tutorials/tutorial.general.md`
  Install aapod identity:
    * helm repo add aad-pod-identity https://raw.githubusercontent.com/Azure/aad-pod-identity/master/charts
    * helm install aad-pod-identity aad-pod-identity/aad-pod-identity (STEP ONE)


  Download https://raw.githubusercontent.com/Azure/aad-pod-identity/master/deploy/infra/mic-exception.yaml
  Change downloaded manifest, and change namespace to : `kube-system` (https://github.com/Azure/aad-pod-identity/issues/671)
  Then kubectl apply -f "PATH_TO_DOWNLOADED_AND_CHANGED_MANIFEST.yaml"
  Next, apply `kubernetes_yamls/mic_exception.yaml` (STEP TWO)
  helm install -f helm-config.yaml application-gateway-kubernetes-ingress/ingress-azure --generate-name (STEP THREE)
  #for that^ use agw-identity-dev to auth, after that apply agic.yaml and you should have managed agw controller that does resoruces in cluster.
```
## Rules disabled on WAF on AGW because of 403 errors on gateway:
- disable RULE 931-APPLICATION-ATTACK-RFI
- disable RULE 942130 (SQLI tautology)
- disable RULE 942430 (SQLI character anomaly)

## AGIC INFO (Azure Gateway Ingress Controller)
- To reconfigure App Gateway use: `kubernetes_yamls/agic.yaml`
- AGIC manages deployed Application Gateway, so terraform script should only deploy bare-bone App Gateway resource, with SSL-cert, so AGIC can access it by-name.
- If you want to add further functionality to Application Gateway - Provide special annotations, like `appgw.ingress.kubernetes.io/appgw-ssl-certificate: "ssl"` 
- Following annotations may be used: `https://azure.github.io/application-gateway-kubernetes-ingress/annotations/`
- AGIC currently uses Self-signed certificate from key vault, later we will use CA signed cert to use on our solution.

Servicebus - enable resource provider servicebus on target subscription {vpnhub_subscription}
```
Failure responding to request: StatusCode=400 -- Original Error: autorest/azure: Service returned an error. Status=400 Code="BadRequest" Message="The client '3a3f6d1d-d042-4cd4-953f-77a4506858ca' with object id '3a3f6d1d-d042-4cd4-953f-77a4506858ca' does not have authorization to perform action 'Microsoft.Network/virtualNetworks/taggedTrafficConsumers/validate/action' over scope '/subscriptions/115e50a3-131b-454b-b168-1aedb4ade9c3/resourceGroups/rg-vpnhub-shared/providers/Microsoft.Network/virtualNetworks/vnet-vpnhub-shared/taggedTrafficConsumers/Microsoft.ServiceBus'
```
* IN CASE OF FAILURE OF HELM INSTALL helm-config application-gateway-kubernetes-ingress/ingress-azure with error `Rendered manifests contain a resource that already exists.` provide name in output like:
helm install -f .\helm-config.yaml `ingress-azure-1644354642` application-gateway-kubernetes-ingress/ingress-azure


# LOGGING FOR AZURE
https://docs.microsoft.com/en-us/azure/application-gateway/application-gateway-diagnostics - for query unhealthy backend
alerts module, write proper documentation !!!

# GANACHE
We use static IP addressess for Ganache services - Polygon/Eth. They use next 2 free addressess (counted from the and, and after last 2 available addressess in the kubernetes aks subnet)

# GRAFANA
Grafana available on: https://mosaico.grafana.net/
To properly run Grafana, you need:
- Application insights tied with AKS
- Log Workspace (container workspace etc. Look into modules/aks, and look for resources such as log_analytic_workspace,etc.)
- Role assignment: Monitoring Reader. (Apply it to Service principal - sp-mosaico-dev/test/prod, to destination: resource group which holds AKS cluster).

To configure Grafana&Prometheus Locally, here are steps to take:
Tutorial used: `https://shailender-choudhary.medium.com/monitor-azure-kubernetes-service-aks-with-prometheus-and-grafana-8e2fe64d1314`
- helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
- kubectl create ns prometheus
- helm install prometheus prometheus-community/kube-prometheus-stack -n prometheus
- kubectl port-forward -n prometheus prometheus-prometheus-kube-prometheus-prometheus-0 9090
- kubectl get secret -n prometheus prometheus-grafana -o=jsonpath='{.data.admin-user}' GET ADMIN USER LOGIN
- kubectl get secret -n prometheus prometheus-grafana -o=jsonpath='{.data.admin-password}'

Last two steps are to obtain credentials, to log into grafana locally. After installing packages with helm, you cant port forward Cluster to Local port, to run Grafana & Prometheus.
- kubectl port-forward -n prometheus [POD-NAME]-0 9090
- kubectl port-forward -n prometheus [POD-NAME] 3000

# RABBITMQ
(credits to: https://medium.com/microsoftazure/functions-to-kubernetes-with-keda-rabbitmq-fd41e9f34feb)
To set up rabbit mq on cluster first:
- helm repo add bitnami https://charts.bitnami.com/bitnami
- helm search repo bitnami
- kubectl create namespace rabbitmq
- helm show values bitnami/rabbitmq > values.yaml (this will be used  to swap some variables in helm chart)
- helm install rabbitmq bitnami/rabbitmq --namespace rabbitmq -f ./values.yaml
- `!IMPORTANT! remember that this helm chart creates PVC - persistent volume claim, when you delete chart, it values may persist, thus you can delete claim by using:`\n
kubectl delete pvc data-rabbitmq-0 (name may vary, you can also invoke first kubectl get pvc)
In this repo, there is already configured `values.yaml`, if you are wondering, the `user` and `password` was changed (line 117,121). On top of that, I've modified the `service` part of chart, changing service type to `LoadBalancer` (line 758), and adding proper annotation for `Internal Loadbalancer` (line 857/858)

Alternative when configuring cluster:
(https://www.rabbitmq.com/kubernetes/operator/using-operator.html#configure)
- helm show values bitnami/rabbitmq-cluster-operator > values_cluster.yaml
- helm install rabbitmq-cluster bitnami/rabbitmq-cluster-operator --namespace rabbitmq -f ./values_cluster.yaml 
- kubectl apply -f definition.yaml 
# AZURE SQL SERVER ADDING USERS:
To add users, connect to admin account to our database dev/prod and do the following:

After logging in to ssms (SQL Server Management Studio (SSMS), on top, near icon `change-connection`, select `master` database and execute following query on it:

```
CREATE LOGIN [YOUR_LOGIN_HERE] WITH password='[YOUR_PASSWORD_HERE]';
```
Then create new queries, selecting other databases (sqldb-identity-dev) and so on:
```
CREATE USER [YOUR_USERNAME_HERE] FROM LOGIN [YOUR_LOGIN_HERE];
EXEC sp_addrolemember N'db_datareader', N'[YOUR_USERNAME_HERE]'
GO
```
Login & Username can be the same.
User will be created with role `db_datareader`

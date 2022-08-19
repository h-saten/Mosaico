  
# Setting up Self-hosted runner
- create following secrets:
  * kubectl create secret docker-registry acr-secret --namespace mosaico --docker-server=`ACR FQDN` --docker-username=`ACR NAME` --docker-password=`INSERT ACR PASSWORD`
  * kubectl create secret generic controller-manager -n mosaico --from-literal=pat=`INSERT GITHUB PAT HERE` (Needs access to repo, and some more if necessary)
- then to create self hosted runner: `kubectl apply -f aks-runner.yaml`
- If there is need, you can add replicasets for self hosted runner
- you need for the PAT - REPO ADMIN, AND DEPLOY IT IN MOSACICO NAMESPACE!!!!!!!!
- more info on https://github.com/actions-runner-controller/actions-runner-controller
## there is also acr_push_scan.yaml that can be made as a separate action to build and push self-hosted runner to acr
- to update runner you can use acr_push_scan custom action, or do it manually with docker build / docker push to acr repo

IF SELF HOSTED FAILS WITH ERROR An error occurred: Not configured. Run config.(sh/cmd) to configure the runner. 
UPDATE THE SECRET `controller-manager` WITH A NEW PERSONAL ACCESS TOKEN (REPO+ADMIN)

kubectl create secret generic controller-manager -n mosaico --from-literal=pat=`INSERT GITHUB PAT HERE`

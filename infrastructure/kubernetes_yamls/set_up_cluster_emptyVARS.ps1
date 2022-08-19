$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath

kubectl create -f "$dir/mosaico_namespace.yaml"

kubectl config set-context --current --namespace=mosaico

helm repo add aad-pod-identity https://raw.githubusercontent.com/Azure/aad-pod-identity/master/

helm install aad-pod-identity aad-pod-identity/aad-pod-identity

kubectl apply -f "$dir/mic_exception.yaml" 

helm install -f "$dir/helm-config_vars.yaml" application-gateway-kubernetes-ingress/ingress-azure --generate-name 

kubectl apply -f "$dir/agic.yaml"

kubectl create secret generic controller-manager -n mosaico --from-literal=pat=[INSERT_GITHUB_ACCESS_KEY_WITHOUT_QUOTES_AND_BRACKETS_HERE]

kubectl apply -f "$dir/../self_hosted_runner/aks-runner.yaml"
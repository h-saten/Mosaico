$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath

kubectl delete -f "$dir/aks-runner.yaml"

kubectl delete secret controller-manager -n mosaico

helm del $(helm list --all --short)

kubectl delete -f "$dir/mic_exception.yaml"

kubectl delete --all deployments --namespace=mosaico

kubectl delete --all pods --namespace=mosaico

kubectl delete -f "$dir/mosaico_namespace.yaml"
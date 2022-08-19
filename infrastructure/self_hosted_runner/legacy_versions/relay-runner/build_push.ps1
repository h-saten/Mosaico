$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath

$semver = "$dir/runner_semver.txt"
$versionparts = (get-content -Path $semver).split('.')
([int]$versionparts[-1])++
$versionparts -join('.') | set-content $semver

$updated_semver = (get-content -Path $dir/runner_semver.txt)

az acr login --name acrmosaico.azurecr.io

docker build "$dir/" --tag acrmosaico.azurecr.io/aks-runner-mosaico-relay-prod:$updated_semver

docker tag acrmosaico.azurecr.io/aks-runner-mosaico-relay-prod:$updated_semver acrmosaico.azurecr.io/aks-runner-mosaico-relay-prod:latest

docker push acrmosaico.azurecr.io/aks-runner-mosaico-relay-prod:$updated_semver

docker push acrmosaico.azurecr.io/aks-runner-mosaico-relay-prod:latest

# kubectl delete -f "$dir/aks-runner.yaml"

# kubectl apply -f "$dir/aks-runner.yaml"

# $pods = $(kubectl get pods -l app=github-runner -n mosaico --no-headers -o custom-columns=":metadata.name" | findstr github)




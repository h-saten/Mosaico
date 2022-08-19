$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath

kubectl delete -f $dir/dev/ingress.yaml

kubectl apply -f $dir/dev/ingress.yaml

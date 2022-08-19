# HUGINN
https://blog.true-kubernetes.com/automatically-search-craigslist-using-huginn-and-kubernetes/

kubectl create secret generic huginn-db-secret --from-literal=POSTGRES_USER=root --from-literal=POSTGRES_PASSWORD=[PASSWORD]
kubectl create secret generic huginn-server-secret --from-literal=DATABASE_USERNAME=root --from-literal=DATABASE_PASSWORD=[PASSWORD]

REMEMBER TO APPLY DATABASE MANIFEST FIRST, AND LET IT SPOOL UP BEFORE RUNNING SERVER!!

Useful articles/blogs:
https://github.com/huginn/huginn/wiki/Formatting-Events-using-Liquid#filters-added-by-huginn
https://edlinus.cn/archives/monitor-changes-in-a-website-and-push-notification-to-slack-via-huginn-and-slack-api.html

# JOBS
cli_migrate_database_core/id - uses same --prod arg for now, later might be changed for specific --dev/--prod

# Monitoring
To configure how Log Analytics workspace refer to this tutorial: `https://docs.microsoft.com/en-us/azure/azure-monitor/containers/container-insights-agent-config`
Appropiate file is located in `./infrastructure/kubernetes_yamls/dev/container-azm-ms-agentconfig.yaml` and `./infrastructure/kubernetes_yamls/prod/container-azm-ms-agentconfig.yaml`
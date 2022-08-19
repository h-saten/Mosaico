#How to run loki instance:
- https://grafana.com/orgs/mosaico/hosted-logs/195928#sending-logs
- Proceed with the guide, and finally apply following command to desired environment, where Loki + Promtail should run. (Grafana free really offers only one environment to monitor, because when you want to ingest logs from other env, you have to configure new grafana stack to get new URL, which is destination of logs ingestion (logs-prod-....))
curl -fsS https://raw.githubusercontent.com/grafana/loki/master/tools/promtail.sh | sh -s [USER_LOGIN] [APIKEY] [GRAFANA LOGS URL] grafana-loki | kubectl apply --namespace=grafana-loki -f  -


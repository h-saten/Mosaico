#!/usr/bin/powershell -Command
python.exe rabbitmqadmin -f tsv -q list queues name > rabbitmq-queues.txt
foreach($line in Get-Content .\rabbitmq-queues.txt) {
    python.exe rabbitmqadmin --host=localhost --port=15672 --ssl-disable-hostname-verification --username=guest --password=guest purge queue name=$line
}
del rabbitmq-queues.txt
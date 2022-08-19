# Testing locally
Use [Azurite](https://github.com/Azure/Azurite) to emulate blob storage on local machine. Run `npm install -g azurite` to install it. Create a folder somewhere on disk where files will be stored. And then to run blob storage client run:

`azurite-blob -s -l d:\azurite -d d:\azurite\debug.log --blobPort 8143`

Use following connection string to access emulator:

`DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:8143/devstoreaccount1;`
#!/bin/bash

GITHUB_OWNER=$GITHUB_OWNER
GITHUB_PAT=$GITHUB_PAT

REG_TOKEN=$(curl -sX POST -H "Authorization: token ${GITHUB_PAT}" https://api.github.com/orgs/${GITHUB_OWNER}/actions/runners/registration-token | jq .token --raw-output)

cd /home/docker/actions-runner

./config.sh --url https://github.com/${GITHUB_OWNER} --token ${REG_TOKEN} --labels self-hosted,Linux,X64,${ENVIRONMENT}

cleanup() {
    echo "Removing runner..."
    ./config.sh remove --unattended --token ${REG_TOKEN}
}

trap 'cleanup; exit 130' INT
trap 'cleanup; exit 143' TERM

./run.sh & wait $!
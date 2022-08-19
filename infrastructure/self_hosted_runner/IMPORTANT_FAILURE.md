IN CASE OF FAILURE OF CREATING GITHUB SELF HOSTED RUNNER:
go to :
https://github.com/sapiency-io/mosaico-reference/settings/actions/runners/new

and copy token from configure tab, and paste it: to entrypoint.sh

## UPDATE:
Mosaico now has self-hosted `ORGANISATION` runner. Make sure that secret on kubernetes `controller-manager` has proper Personal Access Token with:
- REPO SCOPE - ALL
- ADMIN SCOPE - ALL
This PAT is required to GET token to auth runner, from github api. (https://docs.github.com/en/rest/actions/self-hosted-runners#create-a-remove-token-for-an-organization)
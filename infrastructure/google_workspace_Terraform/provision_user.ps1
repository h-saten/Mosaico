$FileName = ".\passwords.txt"
if (Test-Path $FileName) {
  Remove-Item $FileName
}

python .\gworkspace.py
# terraform apply --var-file="./terraform.tfvars" --parallelism=1 --auto-approve
terraform apply --var-file="./terraform.tfvars" --auto-approve
terraform output password > passwords.txt
python .\send_invitation.py
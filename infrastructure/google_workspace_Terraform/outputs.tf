output "password" {
  sensitive = true
  value = [
    for bd in random_password.password : bd.result
  ]
}
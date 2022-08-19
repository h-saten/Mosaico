locals {
  users = csvdecode(file("${path.module}/users.csv"))
  groups = csvdecode(file("${path.module}/groups.csv"))
}

resource "random_password" "password" {
  for_each = { for user in local.users : user.index => user }
  length           = 12
  special          = true
  override_special = "_%@"
}

resource "googleworkspace_user" "users" {
  for_each = { for user in local.users : user.recovery_email => user }

  primary_email = format(
    "%s%s%s@%s",
    replace(replace(replace(replace(replace(replace(replace(substr(lower(each.value.first_name), 0, 1),"ł","l"),"ą","a"),"ę","e"),"ć","c"),"ó","o"),"ż","z"),"ź","z"),
    ".",
    replace(replace(replace(replace(replace(replace(replace(lower(each.value.last_name),"ł","l"),"ą","a"),"ę","e"),"ć","c"),"ó","o"),"ż","z"),"ź","z"),
  var.domain)

  org_unit_path = each.value.org_unit_path

  password = md5(random_password.password[each.value.index].result)

  hash_function = "MD5"

  change_password_at_next_login = true

  name {
    family_name = each.value.last_name
    given_name  = each.value.first_name
  }

  recovery_email = each.value.recovery_email
}

# # 017dp8vu20whn92
# assigning users to groups
resource "googleworkspace_group_member" "sales" {
  depends_on = [
    googleworkspace_user.users
  ]
  for_each = { for user in local.groups : user.index => user }
  group_id = each.value.group == "all-mosaico" ? "017dp8vu20whn92": each.value.group == "all-copernic" ? "00tyjcwt2cwai4a" : "019c6y182qs9pg1"
  email = format(
    "%s%s%s@%s",
    replace(replace(replace(replace(replace(replace(replace(substr(lower(each.value.first_name), 0, 1),"ł","l"),"ą","a"),"ę","e"),"ć","c"),"ó","o"),"ż","z"),"ź","z"),
    ".",
    replace(replace(replace(replace(replace(replace(replace(lower(each.value.last_name),"ł","l"),"ą","a"),"ę","e"),"ć","c"),"ó","o"),"ż","z"),"ź","z"),
  var.domain)
}
# generate random password, then send it thorugh script to receivers in recovery mail so they can log in?
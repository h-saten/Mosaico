import os

cwd = os.getcwd().split("\\")[-1]

if cwd == 'mosaico-reference':

    os.chdir("./infrastructure/terraform")

dir_name = input("provide directory name (for module): ")
dir_name = f"./{dir_name}"

envs = ["dev","test","prod"]
tf_names = ["main.tf","outputs.tf","variables.tf"]

os.chdir("./modules")

if __name__ == "__main__":
    os.mkdir(f"{dir_name}")
    for env in envs:
        created_env_dir = f"{dir_name}/{env}"
        os.mkdir(created_env_dir)
        for terraform_file in tf_names:
            with open(f"{created_env_dir}/{terraform_file}","w") as file_handler:
                file_handler.write("")    
import os
from shutil import copyfile
import sys

cwd = os.getcwd().split("\\")[-1]

if cwd == 'mosaico-reference':

    os.chdir("./infrastructure/terraform")

dir_name = input("provide directory name (for module): ")
decision = input(f"Are you sure that you want to propagate all files from {dir_name} to its /test and /prod directories? Y/N: ")

if decision == "N":
    sys.exit()
else:
    pass

if __name__ == "__main__":

    dir_name = f"{dir_name}"
    prime_path = os.getcwd()

    os.chdir(f"./modules/{dir_name}")
    prime_path += f"\modules\{dir_name}"

    envs = os.listdir()
    envs.remove("dev")

    os.chdir("./dev")
    prime_path += "\dev"
    prime_files = [prime_path + f"\{file}" for file in os.listdir()]

    for env in envs:

        os.chdir("..")
        os.chdir(f"./{env}")
        copied_file = os.getcwd()
        copied_file = [copied_file + f"\{file}" for file in os.listdir()]
        
        for iterator,file in enumerate(copied_file):
            copyfile(prime_files[iterator],file)

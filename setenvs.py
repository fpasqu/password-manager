import os
import secrets
import string

# List of environment variable names
env_var_names = [
    "PM_INSTANCE_ID",
    "PM_TENANT_ID",
    "PM_CLIENT_ID",
    "PM_API_AUDIENCE",
    "PM_CALLBACKPATH",
    "PM_SIGNEDOUT_CALLBACKPATH",
    "PM_GROUP_ID",
    "PM_DB_NAME",
    "PM_IV_KEY"
]

# Get user input for each environment variable
for name in env_var_names:
    if name == "PM_IV_KEY":
        # Generate a random string with letters and numbers
        random_string = ''.join(secrets.choice(string.ascii_letters + string.digits) for _ in range(32))
        print("Generating and setting IV...")
        os.system(f'setx {name} "{random_string}"')
    else:
        user_input_value = input(f'Enter the value for {name}: ')
        os.system(f'setx {name} "{user_input_value}"')

# Notify the user
input('Environment variables set. Press Enter to exit...')
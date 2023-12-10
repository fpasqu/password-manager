import os
import secrets
import string

env_var_names = [
    {
        "PM_TENANT_ID": "Enter the value of your Tenant ID: "
    },{
        "PM_CLIENT_ID": "Enter the value of your Client ID: "
    },{
        "PM_API_AUDIENCE": "Enter the value of your API Audience URI: "
    },{
        "PM_CALLBACKPATH": "Enter the value of your Callback URL: "
    },{
        "PM_GROUP_ID": "Enter the value of your Group ID: "
    },{
        "PM_DB_NAME": "Enter the value of the database name: "
    },{
        "PM_IV_KEY": "Generating and setting IV..."
    },{
        "PM_SALT_KEY": "Generating and setting Salt..."
    }
]

for item in env_var_names:
    key, value = list(item.items())[0]
    if key == "PM_IV_KEY":
        print(value)
        random_string = ''.join(secrets.choice(string.ascii_letters + string.digits) for _ in range(16))
        os.system(f'setx {key} "{random_string}"')
    elif key == "PM_SALT_KEY":
        print(value)
        random_string = ''.join(secrets.choice(string.ascii_letters + string.digits) for _ in range(32))
        os.system(f'setx {key} "{random_string}"')
    else:
        user_input_value = input(value)
        os.system(f'setx {key} "{user_input_value}"')

input('Environment variables set. Press Enter to exit...')
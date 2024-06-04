import yaml
from typing import List
from Users import UserModel, UserListModel
import csv
import json
import xml.etree.ElementTree as ET


def load_users_from_yaml(file_path: str) -> UserListModel:
    with open(file_path, 'r') as file:
        yaml_data = yaml.safe_load(file)
        users_data = yaml_data.get('Users', [])

    users = []
    for user_entry in users_data:
        user = UserModel(
            FirstName=user_entry.get('FirstName'),
            LastName=user_entry.get('LastName'),
            Valid=user_entry.get('Valid'),
            Address=user_entry.get('Address'),
            Age=user_entry.get('Age'),
            Education=user_entry.get('Education', [])
        )
        users.append(user)

    return UserListModel(Users=users)

def load_users_from_csv(file_path: str) -> UserListModel:
    users = []
    with open(file_path, newline='') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            user = UserModel(
                FirstName=row['FirstName'],
                LastName=row['LastName'],
                Valid=bool(row['Valid']),  # Convert to boolean
                Address=row['Address'],
                Age=int(row['Age']),  # Convert to integer
                Education=[value for key, value in row.items() if key.startswith('Education__') and value]  # Get all non-empty Education fields
            )
            users.append(user)
    return UserListModel(Users=users)

def load_users_from_json(file_path: str) -> UserListModel:
    with open(file_path, 'r') as file:
        json_data = json.load(file)
    
    users = []
    for user_entry in json_data.get('Users', []):
        user = UserModel(
            FirstName=user_entry.get('FirstName'),
            LastName=user_entry.get('LastName'),
            Valid=user_entry.get('Valid'),
            Address=user_entry.get('Address'),
            Age=user_entry.get('Age'),
            Education=user_entry.get('Education', [])
        )
        users.append(user)

    return UserListModel(Users=users)

def load_users_from_xml(file_path: str) -> UserListModel:
    tree = ET.parse(file_path)
    root = tree.getroot()
    
    users = []
    for user_element in root.findall('UserModel'):
        user = UserModel(
            FirstName=user_element.find('FirstName').text,
            LastName=user_element.find('LastName').text,
            Valid=user_element.find('Valid').text.lower() == 'true',  # Convert to boolean
            Address=user_element.find('Address').text,
            Age=int(user_element.find('Age').text),  # Convert to integer
            Education=[edu.text for edu in user_element.findall('Education')]
        )
        users.append(user)

    return UserListModel(Users=users)

def load_users_from_txt(file_path: str) -> UserListModel:
    users = []
    with open(file_path, 'r') as file:
        lines = file.readlines()

    user_data = {}
    for line in lines:
        line = line.strip()
        if line:
            key, value = line.split(': ')
            if key == "Education":
                if "Education" not in user_data:
                    user_data["Education"] = []
                user_data["Education"].append(value)
            else:
                user_data[key] = value
        elif len(user_data) != 0:
            user = UserModel(
                FirstName=user_data.get('FirstName'),
                LastName=user_data.get('LastName'),
                Valid=user_data.get('Valid') == 'true',  # Convert to boolean
                Address=user_data.get('Address'),
                Age=int(user_data.get('Age', 0)),  # Convert to integer with a default value of 0
                Education=user_data.get('Education', [])
            )
            users.append(user)
            user_data = {}

    return UserListModel(Users=users)

# Example usage:
file_path = 'DataFiles/User.'
user_list_from_yaml = load_users_from_yaml(file_path + 'yaml')
user_list_from_csv = load_users_from_csv(file_path + 'csv')
user_list_from_json = load_users_from_json(file_path + 'json')
user_list_from_xml = load_users_from_xml(file_path + 'xml')
user_list_from_txt = load_users_from_txt(file_path + 'txt')

print('Yaml')
print(user_list_from_yaml.Users)
print('Csv')
print(user_list_from_csv.Users)
print('Json')
print(user_list_from_json.Users)
print('Xml')
print(user_list_from_xml.Users)
print('txt')
print(user_list_from_txt.Users)



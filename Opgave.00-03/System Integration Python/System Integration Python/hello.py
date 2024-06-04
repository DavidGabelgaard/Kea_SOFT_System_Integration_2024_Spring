from flask import Flask
import json
from System_Integration_Python import load_users_from_json, load_users_from_csv, load_users_from_txt, load_users_from_xml, load_users_from_yaml
import requests


app = Flask(__name__)



@app.route("/txt")
def get_users_as_string_from_txt():
    return json.dumps(load_users_from_txt('../../DataFiles/User.txt').to_dict(), indent=4)

@app.route("/json")
def get_users_as_string_from_json():
    return json.dumps( load_users_from_json('../../DataFiles/User.json').to_dict(), indent=4)

@app.route("/csv")
def get_users_as_string_from_csv():
    return json.dumps( load_users_from_csv('../../DataFiles/User.csv').to_dict(), indent=4)

@app.route("/xml")
def get_users_as_string_from_xml():
    return json.dumps( load_users_from_xml('../../DataFiles/User.xml').to_dict(), indent=4)

@app.route("/yaml")
def get_users_as_string_from_yaml():
    return json.dumps( load_users_from_yaml('../../DataFiles/User.Yaml').to_dict(), indent=4)

@app.route("/ASPDOTNET/txt")
def get_users_from_DOTNET_txt():
    response =  requests.get('http://localhost:5148/Users/Txt').json()
    return {"data" : response }

@app.route("/ASPDOTNET/json")
def get_users_from_DOTNET_json():
    response =  requests.get('http://localhost:5148/Users/Json').json()
    return {"data" : response }

@app.route("/ASPDOTNET/csv")
def get_users_from_DOTNET_csv():
    response =  requests.get('http://localhost:5148/Users/Csv').json()
    return {"data" : response }

@app.route("/ASPDOTNET/xml")
def get_users_from_DOTNET_xml():
    response =  requests.get('http://localhost:5148/Users/Xml').json()
    return {"data" : response }

@app.route("/ASPDOTNET/yaml")
def get_users_from_DOTNET_yaml():
    response =  requests.get('http://localhost:5148/Users/Yaml').json()
    return {"data" : response }


if __name__ == "__main__":
    # Run the Flask app on localhost with port 5000
    app.run(debug=True)
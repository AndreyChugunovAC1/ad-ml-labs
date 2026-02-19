import json
from os import listdir
from os.path import isfile, join
from pprint import pprint
import csv

def list_dict_to_csv(data, filename: str, encoding: str = 'utf-8'):
    all_keys = set()
    for item in data:
        all_keys.update(item.keys())
    headers = sorted(all_keys)
    with open(filename, 'w', newline='', encoding=encoding) as csvfile:
        writer = csv.DictWriter(csvfile, fieldnames=headers, delimiter='\t')
        writer.writeheader()
        for item in data:
            row = {key: item.get(key, '') for key in headers}
            writer.writerow(row)

def get_dict_from_json(jsn):
    result = {}
    def extract_properties(obj):
        if isinstance(obj, dict):
            if 'name' in obj and 'value' in obj:
                prop_id = obj['name']
                prop_value = obj['value']
                if prop_value is not None and prop_value != "":
                    result[prop_id] = prop_value
            for value in obj.values():
                extract_properties(value)
        elif isinstance(obj, list):
            for item in obj:
                extract_properties(item)
    extract_properties(jsn)
    return result

teapots_path  = 'teapots'
files = [f for f in listdir(path=teapots_path)]
dicts = []
for i, filename in enumerate(files):
    with open(f"{teapots_path}/{filename}") as f:
        data = json.load(f)
        body = data["body"]
        dicts.append(get_dict_from_json(body))

list_dict_to_csv(dicts, "data.csv")
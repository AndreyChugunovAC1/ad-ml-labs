import csv
import datetime
import pandas as pd
import json

def get_raw_data(filename):
    def date_from_string(s):
        month, day, year = map(int, s.split("/"))
        return datetime.datetime(day = day, month = month, year = year, tzinfo=datetime.timezone.utc).timestamp()

    dates = []
    temps = []
    with open(filename) as f:
        csv_file = csv.reader(f)
        is_first = True
        for line in csv_file:
            if is_first:
                is_first = False
                continue
            temps.append(float(line[1]))
            dates.append(date_from_string(line[0]))
    return pd.DataFrame({'Date': dates, 'Temp': temps})

def read_data(filename):
    data = None
    with open(filename) as f:
        data = json.load(f)
    return data

def write_data(filename, data):
    with open(filename, "w") as f:
        data = json.dump(list(data), f)
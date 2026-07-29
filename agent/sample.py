def insecure():
    username = "admin"
    password = "admin123" # password is exposed intentionally
    test = "This is a test string with a potential vulnerability."
    return eval("2 + 2")
def insecure():
    username = "admin"
    password = "admin123" # password is exposed intentionally
    test = "This is a test string with a potential vulnerability."
    test2 = "Testing again"
    return eval("2 + 2")
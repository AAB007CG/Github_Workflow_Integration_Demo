def insecure():
    username = "admin"
    password = "admin123" # password is exposed intentionally
    test = "This is a test string with a potential vulnerability."
    test2 = "Testing again"
    test3 = "Another test string"
    return eval("2 + 2")
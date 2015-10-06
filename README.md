# flupen
Displays how to find out email addresses linked to an account on the Flubit website. Also serves as a basic example of how to use Selenium WebDriver.

# IMPORTANT
This code is for educational purposes only. This code exploits a flaw in the Flubit UI in hopes that it will get fixed
and therefore ensure security of the users registered on the website.

# Usage
```
flupen.exe $filepath_to_json_dictionary<optional>
```
`$filepath_to_json_dictionary` must point to a JSON file containing a JSON array, such as:
```
[
    "email1@mail.com",
    "email2@mail.com",
    "email3@mail.com"
]
```

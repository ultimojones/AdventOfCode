//var password = "vzbxkghb".ToCharArray();
var password = "vzbxxyzz".ToCharArray();

while (true)
{
    for (int i = password.Length - 1; i >= 0; i--)
    {
        var digit = password[i] + 1;
        if (digit > 'z')
            password[i] = 'a';
        else
        {
            password[i] = (char)digit;
            break;
        }
    }

    if (!password.Any(c => c == 'i' || c == 'l' || c == 'o'))
        if (Enumerable.Range(0, password.Length - 2).Any(i => password[i] + 1 == password[i + 1] && password[i + 1] + 1 == password[i + 2]))
            if (Enumerable.Range(0, password.Length - 3).Any(i => password[i] == password[i + 1] 
                && Enumerable.Range(i + 2, password.Length - 3 - i).Any(j => password[i] != password[j] && password[j] == password[j + 1])))
                break;
}

    Console.WriteLine(new string(password));


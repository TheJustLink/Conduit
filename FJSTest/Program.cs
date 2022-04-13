// See https://aka.ms/new-console-template for more information
using FastJSON;
using FJSTest;

var ser = new Serializator();

Console.WriteLine(TestD(ser));


static string TestD<TObj>(Serializator<TObj> sr)
{
    string text = @"{
    ""firstName"": ""Иван"",
    ""lastName"": ""Иванов"",
    ""address "": {
        ""streetAddress"": ""Московское ш., 101, кв.101"",
        ""city "": ""Ленинград"",
        ""postalCode "": 101101
    },
    ""phoneNumbers"": [
        ""812 123-1234"",
        ""916 123-4567""
    ],
    ""Cards"": [
        1456789,
        6890532.35
    ]
}
";

    TestClass tc = sr.DeserializeReflect<TestClass>(text);
    if (tc.FirstName != "Иван")
        return $"failed read FirstName" + $" ({tc.FirstName})";
    else if (tc.LastName != "Иванов")
        return "failed read LastName" + $" ({tc.LastName})";

    else if (tc.Address is null)
        return "failed read class Address";

    else if (tc.Address.StreetAddress != "Московское ш., 101, кв.101")
        return "failed read class Address.StreetAddress" + $" ({tc.Address.StreetAddress})";
    else if (tc.Address.City != "Ленинград")
        return "failed read class Address.City" + $" ({tc.Address.City})";
    else if (tc.Address.PostalCode != 101101)
        return "failed read class Address.PostalCode" + $" ({tc.Address.PostalCode})";

    else if (tc.PhoneNumbers is null)
        return "failed read array PhoneNumbers" + $" ({tc.PhoneNumbers})";

    else if (tc.PhoneNumbers[0] != "812 123-1234")
        return "failed read array PhoneNumbers[0]" + $" ({tc.PhoneNumbers[0]})";
    else if (tc.PhoneNumbers[1] != "916 123-4567")
        return "failed read array PhoneNumbers[1]" + $" ({tc.PhoneNumbers[1]})";

    else if (tc.Cards is null)
        return "failed read array Cards" + $" ({tc.Cards})";

    else if (tc.Cards[0] != 1456789)
        return "failed read array Cards[0]" + $" ({tc.Cards[0]})";
    else if (tc.Cards[1] != 6890532.35)
        return "failed read array Cards[1]" + $" ({tc.Cards[1]})";

    else return "ok?";
}

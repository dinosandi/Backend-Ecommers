try
{
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        AllowTrailingCommas = true
    };
    
    Console.WriteLine($"Received Items JSON: {request.Items}"); // Debug line
    
    if (string.IsNullOrEmpty(request.Items))
        throw new Exception("Items cannot be empty");

    items = JsonSerializer.Deserialize<List<CreateBundleItemDto>>(request.Items.Trim(), options);
    
    if (items == null || !items.Any())
        throw new Exception("Bundle must contain at least one item");
}
catch (JsonException ex)
{
    Console.WriteLine($"JSON Error: {ex.Message}"); // Debug line
    throw new Exception($"Invalid items format: {ex.Message}");
}
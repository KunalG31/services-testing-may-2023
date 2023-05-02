namespace ProductsApi.Products;

public class SlugGenerator : IGenerateSlugs
{
    private readonly ICheckForUniqueValues _uniquenessChecker;

    public SlugGenerator(ICheckForUniqueValues uniqueChecker)
    {
        _uniquenessChecker = uniqueChecker;
     }

    public async Task<string> GenerateSlugForAsync(string name)
    {
        var config = new Slugify.SlugHelperConfiguration
        {
            CollapseWhiteSpace = true,

        };

        var slugger = new Slugify.SlugHelper(config);
        var attempt = slugger.GenerateSlug(name);

        bool isUnique = await _uniquenessChecker.IsUniqueAsync(attempt);

        if (isUnique)
        {
            return attempt;
        }
        else
        {
            var letters = "abcdefhijklmnopqrstuvwxyz";
            var idx = 0;
            while (idx < letters.Length)
            {
                var retryAttempt = attempt + "-" + letters[idx];
                isUnique = await _uniquenessChecker.IsUniqueAsync(retryAttempt);
                if (isUnique)
                {
                    return retryAttempt;
                }
                else
                {
                    idx++;
                }
            }
            return attempt + Guid.NewGuid().ToString(); // discovered this.
        }
    }
}
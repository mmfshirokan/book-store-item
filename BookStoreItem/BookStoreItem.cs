using System.Globalization;

namespace BookStoreItem
{
    /// <summary>
    /// Represents the an item in a book store.
    /// </summary>
    public class BookStoreItem
    {
        private readonly string authorName;
        private readonly string? isni;
        private readonly bool hasIsni;
        private decimal price;
        private string currency;
        private int amount;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn)
            : this(authorName, title, publisher, isbn, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn, string? isni)
            : this(authorName, title, publisher, isbn, isni, null, string.Empty, 0.0m, "USD", 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn, DateTime? published, string bookBinding, decimal price, string currency, int amount)
            : this(authorName, title, publisher, isbn, null, published, bookBinding, price, currency, amount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookStoreItem"/> class with the specified <paramref name="authorName"/>, <paramref name="isni"/>, <paramref name="title"/>, <paramref name="publisher"/> and <paramref name="isbn"/>, <paramref name="published"/>, <paramref name="bookBinding"/>, <paramref name="price"/>, <paramref name="currency"/> and <paramref name="amount"/>.
        /// </summary>
        /// <param name="authorName">A book author's name.</param>
        /// <param name="isni">A book author's ISNI.</param>
        /// <param name="title">A book title.</param>
        /// <param name="publisher">A book publisher.</param>
        /// <param name="isbn">A book ISBN.</param>
        /// <param name="published">A book publishing date.</param>
        /// <param name="bookBinding">A book binding type.</param>
        /// <param name="price">An amount of money that a book costs.</param>
        /// <param name="currency">A price currency.</param>
        /// <param name="amount">An amount of books in the store's stock.</param>
        public BookStoreItem(string authorName, string title, string publisher, string isbn, string? isni, DateTime? published, string bookBinding, decimal price, string currency, int amount)
        {
            if (string.IsNullOrWhiteSpace(authorName))
            {
                throw new ArgumentException("authorName is not valid", nameof(authorName));
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("title is not valid", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(publisher))
            {
                throw new ArgumentException("publisher is not valid", nameof(publisher));
            }

            if (!ValidateIsni(isni!))
            {
                throw new ArgumentException("Isni is not valid.", nameof(isni));
            }

            if (!ValidateIsbn(isbn) || !ValidateIsbnChecksum(isbn))
            {
                throw new ArgumentException("Isbn is not valid.", nameof(isbn));
            }

            ThrowExceptionIfCurrencyIsNotValid(currency, this.Currency);

            this.authorName = authorName;
            this.isni = isni;

            if (this.isni != null)
            {
                this.hasIsni = true;
            }
            else
            {
                this.hasIsni = false;
            }

            this.Title = title;
            this.Publisher = publisher;
            this.Isbn = isbn;
            this.Published = published;
            this.BookBinding = bookBinding;
            this.Price = price;
            this.Currency = currency;
            this.Amount = amount;
        }

        /// <summary>
        /// Gets a book author's name.
        /// </summary>
        public string AuthorName => this.authorName;

        /// <summary>
        /// Gets an International Standard Name Identifier (ISNI) that uniquely identifies a book author.
        /// </summary>
        public string? Isni => this.isni;

        /// <summary>
        /// Gets a value indicating whether an author has an International Standard Name Identifier (ISNI).
        /// </summary>
        public bool HasIsni => this.hasIsni;

        /// <summary>
        /// Gets a book title.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a book publisher.
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Gets a book International Standard Book Number (ISBN).
        /// </summary>
        public string Isbn { get; private set; }

        /// <summary>
        /// Gets or sets a book publishing date.
        /// </summary>
        public DateTime? Published { get; set; }

        /// <summary>
        /// Gets or sets a book binding type.
        /// </summary>
        public string BookBinding { get; set; }

        /// <summary>
        /// Gets or sets an amount of money that a book costs.
        /// </summary>
        public decimal Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.price), "Price is less then zero.");
                }

                this.price = value;
            }
        }

        /// <summary>
        /// Gets or sets a price currency.
        /// </summary>
        public string Currency
        {
            get
            {
                return this.currency;
            }

            set
            {
                ThrowExceptionIfCurrencyIsNotValid(value, value);
                this.currency = value;
            }
        }

        /// <summary>
        /// Gets or sets an amount of books in the store's stock.
        /// </summary>
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.price), "Price is less then zero.");
                }

                this.amount = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Uri"/> to the contributor's page at the isni.org website.
        /// </summary>
        /// <returns>A <see cref="Uri"/> to the contributor's page at the isni.org website.</returns>
        public Uri GetIsniUri()
        {
            Uri result, baseUri = new Uri("https://isni.org/isni/");
            if (!Uri.TryCreate(baseUri, this.Isni, out result!))
            {
                throw new InvalidOperationException("Isni is null");
            }

            return result;
        }

        /// <summary>
        /// Gets an <see cref="Uri"/> to the publication page on the isbnsearch.org website.
        /// </summary>
        /// <returns>an <see cref="Uri"/> to the publication page on the isbnsearch.org website.</returns>
        public Uri GetIsbnSearchUri()
        {
            Uri result, baseUri = new Uri("https://isbnsearch.org/isbn/");
            Uri.TryCreate(baseUri, this.Isbn, out result!);
            return result;
        }

        /// <summary>
        /// Returns the string that represents a current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (this.Price > 1000)
            {
                return $"{this.Title}, {this.AuthorName}, ISNI IS NOT SET, ".ToString(CultureInfo.InvariantCulture) + "\"" + this.Price.ToString("N", CultureInfo.InvariantCulture) + "\"" + $" {this.Currency}, {this.Amount}".ToString(CultureInfo.InvariantCulture);
            }

            if (this.Isni == null)
            {
                return $"{this.Title}, {this.AuthorName}, ISNI IS NOT SET, ".ToString(CultureInfo.InvariantCulture) + this.Price.ToString("N", CultureInfo.InvariantCulture) + $" {this.Currency}, {this.Amount}".ToString(CultureInfo.InvariantCulture);
            }

            return $"{this.Title}, {this.AuthorName}, {this.Isni}, ".ToString(CultureInfo.InvariantCulture) + this.Price.ToString("N", CultureInfo.InvariantCulture) + $" {this.Currency}, {this.Amount}".ToString(CultureInfo.InvariantCulture);
        }

        private static bool ValidateIsni(string isni)
        {
            if (isni == null)
            {
                return true;
            }

            if (isni.Length != 16)
            {
                return false;
            }

            foreach (char ch in isni!)
            {
                if (!ValidatorChar(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbn(string isbn)
        {
            if (isbn.Length != 10)
            {
                return false;
            }

            foreach (char ch in isbn)
            {
                if (!ValidatorChar(ch))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateIsbnChecksum(string isbn)
        {
            int cheksum = 0;
            int n = 10;
            foreach (char ch in isbn)
            {
                cheksum += ValidatorInt(ch) * n;
                n--;
            }

            if ((cheksum % 11) == 0)
            {
                return true;
            }

            return false;
        }

        private static void ThrowExceptionIfCurrencyIsNotValid(string currency, string parameterName)
        {
            if (!(currency.Length == 3))
            {
                throw new ArgumentException("Currency is not valid", nameof(currency));
            }

            if (!(char.IsLetter(currency, 0) | char.IsLetter(currency, 1) | char.IsLetter(currency, 2)))
            {
                throw new ArgumentException("Currency is not valid", nameof(currency));
            }
        }

        private static bool ValidatorChar(char a) => a switch
        {
            'X' => true,
            'x' => true,
            '1' => true,
            '2' => true,
            '3' => true,
            '4' => true,
            '5' => true,
            '6' => true,
            '7' => true,
            '8' => true,
            '9' => true,
            '0' => true,
            _ => false,
        };

        private static int ValidatorInt(char a) => a switch
        {
            'X' => 10,
            'x' => 10,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            '0' => 0,
            _ => 69,
        };
    }
}

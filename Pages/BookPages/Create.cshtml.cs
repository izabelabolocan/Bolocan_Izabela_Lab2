using Bolocan_Izabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bolocan_Izabela_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Bolocan_Izabela_Lab2Context _context;

    public CreateModel(Bolocan_Izabela_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        var authorList = _context.Authors.Select(x => new
        {
            x.ID,
            FullName = x.LastName + " " + x.FirstName
        });

        ViewData["AuthorID"] = new SelectList(
            authorList, "ID", "FullName");

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher, "ID", "PublisherName");

        var book = new Book();
        book.BookCategories = new List<BookCategory>();

        PopulateAssignedCategoryData(_context, book);

        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync(
        string[] selectedCategories)
    {
        var newBook = new Book();

        if (selectedCategories != null)
        {
            newBook.BookCategories = new List<BookCategory>();

            foreach (var cat in selectedCategories)
            {
                var catToAdd = new BookCategory
                {
                    CategoryID = int.Parse(cat)
                };

                newBook.BookCategories.Add(catToAdd);
            }
        }

        Book.BookCategories = newBook.BookCategories;

        if (!ModelState.IsValid)
        {
            PopulateAssignedCategoryData(_context, Book);

            var authorList = _context.Authors.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            });

            ViewData["AuthorID"] = new SelectList(
                authorList, "ID", "FullName");

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher, "ID", "PublisherName");

            return Page();
        }

        _context.Book.Add(Book);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}


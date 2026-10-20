
using Bolocan_Izabela_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bolocan_Izabela_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Bolocan_Izabela_Lab2Context _context;

    public EditModel(Bolocan_Izabela_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

        if (Book == null)
        {
            return NotFound();
        }

        PopulateAssignedCategoryData(_context, Book);
        await PopulateListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        int? id, string[] selectedCategories)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookToUpdate = await _context.Book
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(b => b.ID == id);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            b => b.Title,
            b => b.AuthorID,
            b => b.Price,
            b => b.PublishingDate,
            b => b.PublisherID))
        {
            UpdateBookCategories(
                _context, selectedCategories, bookToUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        PopulateAssignedCategoryData(_context, bookToUpdate);
        await PopulateListsAsync();

        return Page();
    }

    private Task PopulateListsAsync()
    {
        var authorList = _context.Authors.Select(x => new
        {
            x.ID,
            FullName = x.LastName + " " + x.FirstName
        });

        ViewData["AuthorID"] = new SelectList(
            authorList, "ID", "FullName", Book.AuthorID);

        ViewData["PublisherID"] = new SelectList(
            _context.Publisher, "ID", "PublisherName", Book.PublisherID);

        return Task.CompletedTask;
    }
}


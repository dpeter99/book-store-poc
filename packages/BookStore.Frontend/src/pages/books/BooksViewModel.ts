import {type Book, BooksService} from "../../services/BooksService.ts";
import type {ServicesT} from "../../App.tsx";
import {createStore} from "zustand/vanilla";


export class BooksViewModel {
	private booksService: BooksService;


	public books = createStore<{books: Book[]}>(()=>({
		books: []
	}));

	constructor(cradle: ServicesT) {
		this.booksService = cradle.booksService;

		this.booksService.getBooks()
			.then((books: Book[]) => {
				this.books.setState({books: books});
			});
	}

	public async addBook(name: string, releaseDate: string): Promise<void> {
		await this.booksService.addBook(name, releaseDate);

		// Refresh the books list
		const books = await this.booksService.getBooks();
		this.books.setState({books: books});
	}

}

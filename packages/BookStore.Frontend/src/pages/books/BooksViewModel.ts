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
	
}

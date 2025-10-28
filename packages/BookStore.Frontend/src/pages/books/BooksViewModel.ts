import {type Book, BooksService} from "@/services/BooksService.ts";
import type {ServicesT} from "../../App.tsx";
import {atom} from "jotai";
import type {Store} from "jotai/vanilla/store";


export class BooksViewModel {
	private readonly booksService: BooksService;
	private readonly store: Store;


	public books = atom<Book[]>((get)=>{
		return get(this.booksService.books);
	});

	constructor(cradle: ServicesT) {
		this.booksService = cradle.booksService;
		this.store = cradle.store;
		
		this.books.debugLabel = "BooksVM Books";
	}

	public async addBook(name: string, releaseDate: string): Promise<void> {
		await this.booksService.addBook(name, releaseDate);
	}

}

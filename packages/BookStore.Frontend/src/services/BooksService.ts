import type {ServicesT} from "@/App.tsx";
import type {QueryClient} from "@tanstack/react-query";
import {createStore} from "zustand/vanilla";

export type Book = {
	id: string
	name: string
	releaseDate: string
}


const books: Book[] = [
	{
		id: '1',
		name: 'test',
		releaseDate: '2021-05-01',
	}
]

export class BooksService {
	private querryClient: QueryClient;


	public books = createStore<{books: Book[]}>(()=>({
		books: []
	}));
	
	constructor(cradle: ServicesT) {
		this.querryClient = cradle.queryClient
		
		
	}
	
	public async getBooks(): Promise<Book[]> {
		return this.querryClient.fetchQuery({
			queryKey: ['books'],
			queryFn: async ()=>{
				//Sleep for a second to simulate network
				await new Promise(resolve => setTimeout(resolve, 1000));
				this.books.setState({books});
				return books;
			}
		})
	}

	public async addBook(name: string, releaseDate: string): Promise<Book> {
		// Sleep to simulate network delay
		await new Promise(resolve => setTimeout(resolve, 500));
		// Send to server
		books.push({
			id: String(books.length + 1),
			name,
			releaseDate
		});

		// Invalidate the books query to trigger a refetch
		await this.querryClient.invalidateQueries({queryKey: ['books']});
	}

}

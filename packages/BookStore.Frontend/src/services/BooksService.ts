import type {ServicesT} from "@/App.tsx";
import type {QueryClient} from "@tanstack/react-query";

export type Book = {
	id: string
	name: string
	releaseDate: string
}


const books: Book[] = [
	{
		id: '1',
		name: 'Book 1',
		releaseDate: '2025-02-02'
	},
	{
		id: '2',
		name: 'Book 2',
		releaseDate: '2025-02-50'
	}
]

export class BooksService {
	private querryClient: QueryClient;
	
	constructor(cradle: ServicesT) {
		this.querryClient = cradle.queryClient
	}
	
	public async getBooks(): Promise<Book[]> {
		return this.querryClient.fetchQuery({
			queryKey: ['books'],
			queryFn: async ()=>{
				//Sleep for a second to simulate network
				await new Promise(resolve => setTimeout(resolve, 1000));
				return books;
			}
		})
	}
	
}

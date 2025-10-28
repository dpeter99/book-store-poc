import type {ServicesT} from "@/App.tsx";
import type {QueryClient} from "@tanstack/react-query";
import {atom} from "jotai";
import type {Store} from "jotai/vanilla/store";
import {QueryObserver} from "@tanstack/query-core";
import type {paths} from "@book-store/frontend_api-client/src/schema";
import type { Client } from "openapi-fetch";

export type Book = {
	id?: string
	title: string
	publishedDate?: string
}

export class BooksService {
	private readonly queryClient: QueryClient;
	private readonly store: Store;
	private readonly apiClient: Client<paths>;

	public books = atom<Book[]>([]);
	private readonly booksObserver: QueryObserver<Book[]>;
	
	constructor(cradle: ServicesT) {
		this.queryClient = cradle.queryClient
		this.store = cradle.store;
		this.apiClient = cradle.apiClient;

		this.booksObserver = new QueryObserver<Book[]>(this.queryClient, { 
			queryKey: ['books'],
			queryFn: async ()=>{
				//Sleep for a second to simulate network
				await new Promise(resolve => setTimeout(resolve, 1000));
				
				const {data} = await this.apiClient.GET("/api/v1/book")
				if(data){
					return data;
				}
				return [];
			}
		});
		this.booksObserver.subscribe(result => {
			if(! result.isPending){
				this.store.set(this.books, result.data!);
			}
		})
	}

	public async addBook(name: string, releaseDate: string): Promise<void> {
		// Sleep to simulate network delay
		await new Promise(resolve => setTimeout(resolve, 500));
		// Send to server
		await this.apiClient.POST('/api/v1/book', { body: {
				title: name,
				publishedDate: releaseDate,
				genre: "asd",
			}
		})

		// Invalidate the books query to trigger a refetch
		await this.queryClient.invalidateQueries({queryKey: ['books']});
	}

}

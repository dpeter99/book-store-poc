import {RouterProvider} from "@tanstack/react-router";
import "7.css/dist/7.css";




const queryClient = new QueryClient({
	defaultOptions: {
		queries: {
			staleTime: Infinity,
		},
	},
})

// This code is only for TypeScript
declare global {
	interface Window {
		__TANSTACK_QUERY_CLIENT__:
			import("@tanstack/query-core").QueryClient;
	}
}

// This code is for all users
window.__TANSTACK_QUERY_CLIENT__ = queryClient;


import {asClass, asValue, type AwilixContainer, createContainer} from "awilix";
import {BooksService} from "./services/BooksService.ts";
import {BooksViewModel} from "./pages/books/BooksViewModel.ts";
import {createContext} from "react";
import {router} from "@/routes.tsx";
import { QueryClient } from "@tanstack/react-query";

export type ServicesT = {
	booksService: BooksService
	booksPageViewModel: BooksViewModel,
	queryClient: QueryClient
}
export const Services = createContext<AwilixContainer<ServicesT>>(null!)

function App() {
	
	const container = createContainer<ServicesT>({})
	container.register({
		booksService: asClass(BooksService).singleton(),
		booksPageViewModel: asClass(BooksViewModel).scoped(),
		queryClient: asValue(queryClient),
	})
	
  return (
    <>
		<Services.Provider value={container}>
			<RouterProvider router={router} />
		</Services.Provider>
    </>
  )
}

export default App

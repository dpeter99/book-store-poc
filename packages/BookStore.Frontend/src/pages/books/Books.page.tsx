import {BooksTable} from "./BooksTable";
import {Services, type ServicesT} from "../../App.tsx";
import {useContext, useRef} from "react";
import type {AwilixContainer} from "awilix";

export const BooksPage = () => {
	const container = useContext(Services)
	const scope = useRef<AwilixContainer<ServicesT>>(container.createScope());
	
	
	return (
		<Services.Provider value={scope.current}>
			<div className="p-2">
				<BooksTable />
				<div className="h-4" />
			</div>
		</Services.Provider>
	);
};

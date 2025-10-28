import * as fs from "fs";
import openapiTS, {astToString} from "openapi-typescript";
import ts from "typescript";

const getTypeRefNode = (type: string | string[], brand: string) =>{

	return ts.factory.createTypeReferenceNode(
		ts.factory.createIdentifier("Brand"),
		[
			ts.factory.createTypeReferenceNode((typeof type === 'string')? type : type[0] ),
			ts.factory.createLiteralTypeNode(ts.factory.createStringLiteral(brand)),
		],
	);
	
}
	


const yamlDoc = fs.readFileSync(process.stdin.fd, "utf-8").toString();

const ast = await openapiTS(yamlDoc, {
	transform: (node) => {
		if (node.format?.startsWith("brand::")) {
			const brandType = node.type ?? "string";
			const brandName = node.format?.split("::")[1];
			return getTypeRefNode(brandType, brandName);
		}
		return undefined;
	},
});

const contents = astToString(ast);

// (optional) write to file
fs.writeFileSync("./src/schema.d.ts", contents);
	


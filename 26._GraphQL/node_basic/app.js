import express from "express"
import { GraphQLSchema, GraphQLObjectType, GraphQLString } from 'graphql';

const app = express();

app.use(express.static("public"))

const PORT = process.env.PORT ?? 8080;
app.listen(PORT, () => console.log("Servicer is running on port", PORT));

const schema = new GraphQLSchema({
    query: new GraphQLObjectType({
      name: 'Query',
      fields: {
        hello: {
          type: GraphQLString,
          resolve: () =>(soruce, args, context) => {
            console.log(soruce, args);
            console.log(context)
            return 'world'
          },
        },
      },
    }),
  });

  import { createHandler } from 'graphql-http/lib/use/http';
app.all('/graphql', createHandler({ schema }));



import express from "express"
const app = express();

import swaggerJSDoc from "swagger-jsdoc";
    const SwaggerDefinition = {
        definition: {
        openapi: '3.0.0',
        info: {
            title: 'Hello World',
            version: '1.0.0',
        },
    },
    apis: ["./routers/*.js"],
};

const swaggerOptions = {
    SwaggerDefinition,
    apis: ["./routers/*.js"],
}

import swaggerUI from "swagger-ui-express";
app.use("/docs", swaggerUI.serve, swaggerUI.setup(swaggerJSDoc(SwaggerDefinition)));



import usersRouter from "./routers/usersRouter.js";
app.use(usersRouter);

const PORT = process.env.PORT ?? 8080;
app.listen(PORT, () => console.log("Server is running on port: ", PORT));


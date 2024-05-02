from fastapi import FastAPI

app = FastAPI()

from routes import spacecraft_router
app.include_router(spacecraft_router)

import json

 
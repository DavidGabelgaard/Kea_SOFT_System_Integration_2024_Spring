from fastapi import FastAPI, Form, File, UploadFile
import aiofiles
from typing import Optional 

app = FastAPI()


@app.post("/form")
def basic_form(username: str = Form(...), password: str = Form(default=..., min_length=8)):
    print(username, password)
    return { "username": username }

# @app.post("/fileform")
# def file_form(file: bytes = File(...)):
#     with open('file', 'wb') as f:
#         f.write(file)
#     return {"message": "File Uploaded"}


# @app.post("/fileform")
# async def file_form(file: UploadFile = File(...)):
#     contents = await file.read()
#     print(contents)

#     return {"file_name": file.filename}

# @app.post("/fileform")
# async def file_form(file: UploadFile = File(...)):
#     save_filename = file.filename.replace("/", "_").repace("\\", "_")

#     with open (save_filename, "wb") as f:
#         while content := await file.read(1024):
#             f.write(content)

@app.post("/fileform")
async def file_form(file: UploadFile = File(...), description: Optional[str] = Form(None)):
    save_filename = file.filename.replace("/", "_").repace("\\", "_")

    async with aiofiles.open (save_filename, "wb") as f:
        while content := await file.read(1024):
            await f.write(content)



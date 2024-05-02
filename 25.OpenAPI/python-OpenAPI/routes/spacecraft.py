from fastapi import APIRouter, HTTPException
from pydantic import BaseModel

class SpaceCraft(BaseModel):
    id: int
    name: str

spacecrafts = [
    SpaceCraft(id= 1, name="Apollo 13"),
    SpaceCraft(id = 2, name = "Hubble"),
    SpaceCraft(id = 3, name = "ISS"),
    SpaceCraft(id = 4, name = "Voyager"),
]

router = APIRouter()

@router.get("/api/spacecrafts", tags=["spacecrafts"], response_model = list[SpaceCraft])
async def _():
    return spacecrafts

@router.get("/api/spacecrafts/{spacecraft_id}", tags=["spacecrafts"], response_model = SpaceCraft )
async def _(spacecraft_id: int):
    for spacecraft in spacecrafts:
        if spacecraft.id == spacecraft_id:
            return spacecraft
    raise HTTPException(status_code =404, detail="Spacecraft not found")    
    
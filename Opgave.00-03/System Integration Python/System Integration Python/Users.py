from typing import List
from dataclasses import dataclass, field
import xml.etree.ElementTree as ET

@dataclass
class UserModel:
    FirstName: str = None
    LastName: str = None
    Valid: bool = None
    Address: str = None
    Age: int = None
    Education: List[str] = field(default_factory=list)

@dataclass
class UserListModel:
    Users: List[UserModel] = field(default_factory=list)
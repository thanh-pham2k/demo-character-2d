# Phòng 404 — batch bố cục v2

Imagegen tích hợp. Reference: `../batch-01/01-room-404.png` và `../../T-001-gif-assets/year-one-v3/b01-03.png`. Hai lượt: sheet-v1 tạo mới, sheet-v2 chỉnh hai ô trên vì cửa v1 còn trên tường bên. Grid 2×2; canvas (1672, 941); crop (x1,y1,x2,y2) [(7, 7, 829, 463), (843, 7, 1665, 463), (7, 477, 829, 934), (843, 477, 1665, 934)]; lề crop 7px; thứ tự trống ngày, trống đêm, có nội thất ngày, có nội thất đêm. Pivot/PPU chưa chốt.

## Prompt tạo

Create exactly one 2 by 2 contact sheet of four equal 16:9 landscape game scene panels, straight black #000000 grid separators, no gaps and no text. Use references: image 1 visual style and empty room, image 2 approved composition with loft beds and desks. Same fixed camera and architecture in all four panels: worn yellow dorm room, back wall with barred window slightly left of center, door on SAME BACK WALL to the RIGHT of window, light switch beside door, BOTH side walls free of doors. Wide central clear walking corridor from foreground to door. 01 top left: empty architectural room day, no furniture or characters. 02 top right: same empty architectural room night, no furniture or characters. 03 bottom left: same room daytime, one loft bed against left side wall and another loft bed against right side wall, each with ONE upper mattress and a desk under it, no lower mattresses, two separate laptops, ladders on sides that do not block the center aisle, no characters. 04 bottom right: exactly same furnished room layout at night, no characters. Door must be unobstructed and both lofts fully inside room. Consistent 2D chibi pixel game art, matte, no text, no duplicated furnishings. This is a layout concept to compare clearances, not a production asset.

## Prompt chỉnh

Edit the supplied 2x2 storyboard sheet. Keep bottom two panels unchanged. In BOTH TOP PANELS ONLY correct the architectural back wall to match the bottom panels: the door and switch must be on the back wall immediately to the right of the window, in exactly the same position and camera coordinates as bottom panels. Remove the door and switch from the side wall, replacing them with uninterrupted yellow plaster. Keep top panels empty with no furniture, one day and one night. Preserve 2x2 grid and exact same camera perspective, window, floor, pixel-art style and lighting, no text.

## Giới hạn

Ảnh là reference, cửa/công tắc còn gộp vào nền; hai ô nội thất bị cắt ở mép ảnh và đồ vật gộp. Kích thước, camera và vị trí cửa giữa hai hàng chưa khớp tuyệt đối; chưa kiểm tra placement, collider, sorting, PPU trong Unity. Không dùng làm asset tách lớp cuối.

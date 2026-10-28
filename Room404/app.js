const room = document.querySelector('#room');
const details = document.querySelector('#details');
const reference = document.querySelector('#reference');
const opacity = document.querySelector('#opacity');
let selected = null;
let mode = 'off';
const interactables = [];

function showObjectInfo(object) {
  details.replaceChildren();
  const title = document.createElement('h2');
  title.textContent = object.name;
  const list = document.createElement('dl');
  list.className = 'details-list';
  for (const [label, value] of [['ID', object.id], ['Action', object.action]]) {
    const row = document.createElement('div');
    const term = document.createElement('dt');
    const description = document.createElement('dd');
    term.textContent = label;
    description.textContent = value;
    row.append(term, description);
    list.append(row);
  }
  details.append(title, list);
}
function selectObject(object, element) {
  if (selected) { selected.classList.remove('selected'); selected.setAttribute('aria-pressed', 'false'); }
  selected = element;
  selected.classList.add('selected');
  selected.setAttribute('aria-pressed', 'true');
  showObjectInfo(object);
}
function updateReference() {
  reference.style.display = mode === 'off' ? 'none' : 'block';
  reference.style.opacity = mode === 'overlay' ? opacity.value / 100 : 1;
  document.querySelector('#opacity-label').hidden = mode !== 'overlay';
}
document.querySelectorAll('.mode').forEach(button => button.addEventListener('click', () => {
  mode = button.dataset.mode;
  document.querySelectorAll('.mode').forEach(item => { item.classList.toggle('active', item === button); item.setAttribute('aria-pressed', String(item === button)); });
  updateReference();
}));
opacity.addEventListener('input', updateReference);
fetch('layout.json').then(response => {
  if (!response.ok) throw new Error('Could not load layout.json');
  return response.json();
}).then(layout => {
  document.querySelector('#background').src = layout.background;
  reference.src = layout.reference;
  for (const object of layout.objects) {
    const element = document.createElement(object.clickable ? 'button' : 'div');
    if (object.clickable) element.type = 'button';
    element.className = 'object';
    element.dataset.id = object.id;
    if (object.clickable) {
      element.setAttribute('aria-label', object.name + ', ' + object.id);
      element.setAttribute('aria-pressed', 'false');
    } else {
      element.classList.add('noninteractive');
      element.setAttribute('aria-hidden', 'true');
    }
    Object.assign(element.style, {left: `${object.x * 100}%`, top: `${object.y * 100}%`, width: `${object.width * 100}%`, height: `${object.height * 100}%`, zIndex: object.zIndex});
    if (object.clipPath) element.style.clipPath = object.clipPath;
    if (object.asset) {
      const image = document.createElement('img');
      image.src = object.asset;
      image.alt = '';
      image.draggable = false;
      image.className = 'object-image';
      if (object.mirror) image.classList.add('mirrored');
      element.append(image);
    }
    if (object.clickable) {
      interactables.push({ object, element });
      element.addEventListener('click', () => selectObject(object, element));
    }
    room.append(element);
  }
}).catch(error => { details.textContent = `Could not load room: ${error.message}. Serve this folder over HTTP.`; });

// Character coordinates use the same normalized room space as the layout.
const luon = document.querySelector('#luon');
const checkpoint = document.querySelector('#checkpoint');
const interactionHint = document.querySelector('#interaction-hint');
let nearby = null;
let seat = null;
let tabletSession = null;
let wearingBackpack = false;
let carriedBackpackId = null;
let studying = false;
const deskPanel = document.querySelector('#desk-panel');
const deskChoice = document.querySelector('#desk-choice');
const timingPanel = document.querySelector('#timing-game');
const timingResult = document.querySelector('#timing-result');
const sessionStats = { minutes: 8 * 60, study: 0, fun: 0 };
let deskGame = null;
function renderStats() {
  const hours = Math.floor(sessionStats.minutes / 60) % 24;
  const minutes = sessionStats.minutes % 60;
  document.querySelector('#session-stats').textContent = `Thời gian: ${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')} · Học: ${sessionStats.study} · Vui vẻ: ${sessionStats.fun}`;
}
function showDeskMenu() {
  if (!seat) return;
  deskGame = null; studying = false; walkingTime = 0;
  deskPanel.hidden = false; deskChoice.hidden = false; timingPanel.hidden = true; timingResult.hidden = true;
  setSprite(seatedSprite(0)); updateNearby();
}
function startDeskGame(kind) {
  if (!seat) return;
  deskGame = { kind, phase: 'playing', elapsed: 0, hits: 0, attempts: 0, lastHit: -1, marker: 0 };
  deskPanel.dataset.activity = kind;
  studying = true; walkingTime = 0; keys.clear();
  deskPanel.hidden = false; deskChoice.hidden = true; timingPanel.hidden = false; timingResult.hidden = true;
  document.querySelector('#timing-title').textContent = kind === 'study' ? '📚 Giữ tập trung' : '🎮 Canh thời điểm';
  document.querySelector('#timing-instruction').textContent = kind === 'study' ? 'Bấm khi vạch vàng vào vùng xanh để tập trung học.' : 'Bấm khi vạch vàng vào vùng xanh để ghi điểm.';
  document.querySelector('#timing-marker').style.left = '0%';
  document.querySelector('#desk-progress-fill').style.width = '0%';
  document.querySelector('#timing-status').textContent = 'Còn 6.0 giây · Trúng: 0/3';
  document.querySelector('#timing-hit').disabled = false;
  document.querySelector('#timing-hit').focus();
  updateNearby();
}
function hitTiming() {
  if (!deskGame || deskGame.phase !== 'playing' || deskGame.attempts >= 3 || deskGame.elapsed - deskGame.lastHit < 0.3) return;
  deskGame.lastHit = deskGame.elapsed; deskGame.attempts++;
  if (deskGame.marker >= 0.4 && deskGame.marker <= 0.6) deskGame.hits++;
  document.querySelector('#timing-hit').disabled = deskGame.attempts >= 3;
}
function finishDeskGame(skipped = false) {
  if (!deskGame || deskGame.phase !== 'playing') return;
  deskGame.skipped = skipped;
  deskGame.phase = 'finishing'; deskGame.finishTime = 0;
  timingPanel.hidden = true; deskChoice.hidden = true;
  timingResult.hidden = false;
  document.querySelector('#result-text').textContent = 'Nhân vật đang hoàn tất hoạt động…';
  document.querySelector('#result-continue').hidden = true;
}
function completeDeskGame() {
  const game = deskGame;
  if (!game || game.phase !== 'finishing') return;
  const reward = game.skipped ? 2 : 1 + game.hits;
  sessionStats.minutes += 20;
  if (game.kind === 'study') sessionStats.study += reward;
  else sessionStats.fun += reward;
  renderStats();
  game.phase = 'result'; studying = false;
  const rating = game.skipped ? 'Tự động' : game.hits === 3 ? 'Xuất sắc' : game.hits ? 'Hoàn thành' : 'Cần luyện thêm';
  document.querySelector('#result-text').textContent = `${rating}${game.skipped ? '' : ` · Trúng ${game.hits}/3`} · +20 phút · ${game.kind === 'study' ? 'Học' : 'Vui vẻ'} +${reward}`;
  document.querySelector('#result-continue').hidden = false;
  updateNearby();
}
function tickDeskGame(dt) {
  if (!deskGame || !checkpoint.hidden) return;
  if (deskGame.phase === 'finishing') {
    deskGame.finishTime += dt;
    if (deskGame.finishTime >= 0.8) completeDeskGame();
    return;
  }
  if (deskGame.phase !== 'playing') return;
  deskGame.elapsed += dt;
  const cycle = (deskGame.elapsed * 0.72) % 2;
  deskGame.marker = cycle <= 1 ? cycle : 2 - cycle;
  document.querySelector('#timing-marker').style.left = `${deskGame.marker * 100}%`;
  document.querySelector('#desk-progress-fill').style.width = `${Math.min(100, deskGame.elapsed / 6 * 100)}%`;
  document.querySelector('#timing-status').textContent = `Còn ${Math.max(0, 6 - deskGame.elapsed).toFixed(1)} giây · Trúng: ${deskGame.hits}/${deskGame.attempts} · Lượt: ${deskGame.attempts}/3`;
  if (deskGame.elapsed >= 6) finishDeskGame();
}

let actionSession = null;
let roomLight = true;
const lampLights = { left: true, right: true };
let onBalcony = false;
let journeyZone = null;
let brushed = false;
let refreshed = false;
const journeyScene = document.querySelector('#journey-scene');
const journeyImage = document.querySelector('#journey-image');
const journeyTitle = document.querySelector('#journey-title');
const journeyHotspots = document.querySelector('#journey-hotspots');
const journeyNotice = document.querySelector('#journey-notice');
const hallDownStairs = document.querySelector('#hall-down-stairs');
const washActions = document.querySelector('#wash-actions');
const washFaceButton = document.querySelector('#wash-face');
const brushCloseup = document.querySelector('#brush-closeup');
const brushCharacter = document.querySelector('#brush-closeup-character');
const journeyZones = {
  hallway: { title: 'Hành lang tầng 4', image: 'hallway-v2/day.png', points: [
    { id: 'washroom', label: 'WC Nam', x: .259 }, { id: 'neighbor', label: 'Phòng kế bên', x: .466 },
    { id: 'room', label: 'Phòng 404', x: .606 }, { id: 'stairs', label: 'Cầu thang xuống sảnh', x: .91 }
  ] },
  washroom: { title: 'WC Nam', image: 'interiors-v2/wc-day.png', points: [
    { id: 'exit-wc', label: 'Cửa ra hành lang', x: .135 }, { id: 'sink', label: 'Bồn rửa', x: .82 }
  ] },
  lobby: { title: 'Sảnh KTX', image: 'lobby.png', points: [
    { id: 'upstairs', label: 'Lên tầng 4', x: .87 }, { id: 'outdoor', label: 'Ra KTX đến trạm bus', x: .35 }
  ] },
  bus: { title: 'Trạm xe buýt', image: 'bus-stop.png', points: [ { id: 'back-lobby', label: 'Về sảnh KTX', x: .15 } ] }
};
const hallwayScale = 3.28;
function hallwayCamera() { return Math.max(0, Math.min(hallwayScale - 1, position.x * hallwayScale - .5)); }
function isStairTravel() { return ['stairs', 'upstairs'].includes(actionSession?.journeyAction?.kind); }
function journeyDisplayX(x) { return journeyZone === 'hallway' && !isStairTravel() ? x * hallwayScale - hallwayCamera() : x; }
function journeyReach() { return journeyZone === 'hallway' ? .04 : .1; }
function drawJourneyMap() {
  if (!journeyZone) return;
  const hall = journeyZone === 'hallway' && !isStairTravel();
  journeyScene.classList.toggle('stair-travel', isStairTravel());
  journeyScene.classList.toggle('wc-exiting', actionSession?.journeyAction?.kind === 'exit-wc');
  journeyScene.classList.toggle('side-hallway', hall);
  journeyScene.classList.toggle('washroom', journeyZone === 'washroom');
  document.querySelector('#bus-preview').hidden = journeyZone !== 'bus';
  hallDownStairs.hidden = !hall;
  if (hall) {
    hallDownStairs.style.left = `${journeyDisplayX(.822) * 100}%`;
    hallDownStairs.style.width = `${.16 * hallwayScale * 100}%`;
  }
  const nearSink = journeyZone === 'washroom' && Math.abs(position.x - .82) <= journeyReach();
  washActions.hidden = !nearSink || Boolean(actionSession?.journeyAction);
  washFaceButton.disabled = !brushed;
  washFaceButton.title = brushed ? '' : 'Đánh răng trước khi rửa mặt';
  document.querySelector('#hallway-map').hidden = !hall;
  if (hall) document.querySelector('#hallway-player').style.left = `${position.x * 100}%`;
  journeyImage.style.width = hall ? `${hallwayScale * 100}%` : '100%';
  journeyImage.style.left = hall ? `${-hallwayCamera() * 100}%` : '0';
  for (const button of journeyHotspots.children) {
    const point = journeyZones[journeyZone].points.find(entry => entry.id === button.dataset.id);
    if (!point) continue;
    button.style.left = `${journeyDisplayX(point.x) * 100}%`;
    if (point.id === 'sink') button.textContent = !brushed || refreshed ? 'Bồn rửa · Đánh răng' : 'Bồn rửa · Rửa mặt';
    button.classList.toggle('near', Math.abs(position.x - point.x) <= journeyReach());
  }
}
function setJourneyZone(zone, startX = 0.5, notice = '') {
  journeyZone = zone; onBalcony = true; keys.clear();
  actionSession.phase = 'balcony'; actionSession.time = 0;
  position.x = startX; position.y = .88; walkingTime = 0;
  journeyImage.src = `assets/journey/${journeyZones[zone].image}`;
  journeyTitle.textContent = journeyZones[zone].title;
  journeyNotice.textContent = notice;
  journeyHotspots.replaceChildren();
  for (const point of journeyZones[zone].points) {
    const button = document.createElement('button');
    button.type = 'button'; button.className = 'journey-point'; button.style.left = `${point.x * 100}%`;
    button.textContent = point.label; button.dataset.id = point.id;
    button.addEventListener('click', () => {
      if (Math.abs(position.x - point.x) <= journeyReach()) activateJourneyPoint(point);
      else journeyNotice.textContent = 'Dùng A / D đến gần địa điểm, rồi bấm E.';
    });
    journeyHotspots.append(button);
  }
  journeyScene.hidden = false; updateLights(); drawJourneyMap(); drawLuon(); updateNearby();
}
function nearbyJourneyPoint() {
  if (!journeyZone) return null;
  return journeyZones[journeyZone].points.reduce((best, point) => Math.abs(position.x - point.x) < Math.abs(position.x - (best?.x ?? -10)) ? point : best, null);
}
function startJourneyAction(kind) {
  if (!actionSession || actionSession.journeyAction) return;
  keys.clear();
  actionSession.journeyAction = { kind, time: 0, previousY: position.y };
  if (kind === 'brush' || kind === 'wash') { position.x = .82; position.y = .86; }
  if (kind === 'stairs' || kind === 'upstairs') {
    journeyImage.src = `assets/journey/interiors-v2/stairs-${kind === 'stairs' ? 'down' : 'up'}.png`;
    journeyTitle.textContent = kind === 'stairs' ? 'Cầu thang · Đi xuống' : 'Cầu thang · Đi lên';
    position.x = kind === 'stairs' ? .12 : .35;
    position.y = kind === 'stairs' ? .34 : .85;
    drawJourneyMap();
  }
  journeyNotice.textContent = { brush: 'Đang đánh răng…', wash: 'Đang rửa mặt…', stairs: 'Đang xuống cầu thang…', upstairs: 'Đang lên cầu thang…', 'exit-wc': 'Đang mở cửa ra hành lang…' }[kind];
  updateNearby();
}
function activateJourneyPoint(point) {
  if (actionSession?.journeyAction) return;
  if (!point || Math.abs(position.x - point.x) > journeyReach()) return;
  if (point.id === 'room') { finishAction(); return; }
  if (point.id === 'neighbor') { journeyNotice.textContent = 'Phòng kế bên · Chưa có nội dung tương tác trong bản preview.'; return; }
  if (point.id === 'washroom') { setJourneyZone('washroom', .135); return; }
  if (point.id === 'exit-wc') { startJourneyAction('exit-wc'); return; }
  if (point.id === 'sink') { startJourneyAction(!brushed || refreshed ? 'brush' : 'wash'); return; }
  if (point.id === 'stairs') { startJourneyAction('stairs'); return; }
  if (point.id === 'upstairs') { startJourneyAction('upstairs'); return; }
  if (point.id === 'outdoor') { setJourneyZone('bus', .15, 'Trạm xe buýt · Hành trình xe đến H6 chưa có trong bản preview.'); return; }
  if (point.id === 'back-lobby') setJourneyZone('lobby', .35);
}
function leaveJourneyZone() {
  if (journeyZone === 'hallway') finishAction();
  else if (journeyZone === 'washroom') setJourneyZone('hallway', .259);
  else if (journeyZone === 'bus') setJourneyZone('lobby', .35);
  else setJourneyZone('hallway', .91);
}

const actionAnchors = {
  ladder_left: { x: 0.25, y: 0.57 }, ladder_right: { x: 0.75, y: 0.57 },
  bed_left: { x: 0.25, y: 0.57 }, bed_right: { x: 0.75, y: 0.57 },
  light_switch: { x: 0.544, y: 0.40 }, lamp_left: { x: 0.095, y: 0.51 },
  lamp_right: { x: 0.906, y: 0.51 }, door_main: { x: 0.63, y: 0.40 }
};
function updateLights() {
  room.classList.toggle('lights-off', !roomLight);
  room.classList.toggle('lamp-left-off', !lampLights.left);
  room.classList.toggle('lamp-right-off', !lampLights.right);
  room.classList.toggle('balcony-open', onBalcony);
  room.classList.toggle('door-opening', actionSession?.kind === 'door' && ['entering', 'crossing'].includes(actionSession.phase));
}
function actionSprite(name, frame = 0) { return `assets/luon/actions-v3/${name}_${frame}.svg`; }
function journeySprite(kind) { return `assets/luon/journey-v1/${wearingBackpack ? 'bag' : 'plain'}_${kind}.svg`; }
function doorSprite(frame = 0) { return `assets/luon/door-v2/${wearingBackpack ? 'bag' : 'plain'}_${frame}.svg`; }
function sleepSprite(frame = 0) { return `assets/luon/sleep-v1/head_${frame}.svg`; }
function climbSprite(frame = 0) { return `assets/luon/climb-socks-v2/${wearingBackpack ? 'bag' : 'plain'}_${frame % 2}.svg`; }
function startAction(kind, item) {
  const id = item.object.id;
  actionSession = { kind, id, previous: { ...position }, time: 0, phase: 'acting' };
  if (kind === 'backpack') actionSession.pickingUp = !wearingBackpack;
  keys.clear(); walkingTime = 0;
  if (kind === 'climb') {
    actionSession.phase = 'removing-slippers';
    const right = id.endsWith('right');
    position.x = right ? 0.75 : 0.25; position.y = 0.57;
    actionSession.destination = { x: right ? 0.88 : 0.12, y: 0.235 };
  }
  updateNearby();
}
function finishAction() {
  position.x = actionSession.previous.x; position.y = actionSession.previous.y;
  actionSession = null; onBalcony = false; journeyZone = null; journeyScene.hidden = true; keys.clear(); updateLights(); drawLuon(); updateNearby();
}
const keys = new Set();
const spawn = { x: 0.50, y: 0.67 };
const position = { ...spawn };
function walkCycle(variant) {
  return [
    `assets/luon/walk-v3/${variant}_0.svg`,
    `assets/luon/walk-v2/${variant}_1.svg`,
    `assets/luon/walk-v3/${variant}_1.svg`,
    `assets/luon/walk-v2/${variant}_3.svg`
  ];
}
const walkFrames = walkCycle('plain');
const backpackWalkFrames = walkCycle('bag');
function idleSprite() { return `assets/luon/unified-v1/${wearingBackpack ? 'bag' : 'plain'}_0.svg`; }
function seatedSprite(frame = 0) { return wearingBackpack ? `assets/luon/desk-bag-v1/study_${frame % 2}.svg` : `assets/luon/actions-v2/study_${frame}.svg`; }
function tabletSprite(frame = 0) { return wearingBackpack ? `assets/luon/desk-bag-v1/tablet_${frame % 2}.svg` : `assets/luon/actions-v2/tablet_${frame}.svg`; }
let lastFrame = 0;
let walkingTime = 0;
let sprite = '';
let facingLeft = false;
function setSprite(path) {
  if (sprite !== path) { sprite = path; luon.src = path; }
}
function drawLuon() {
  const brush = actionSession?.journeyAction?.kind === 'brush' ? actionSession.journeyAction : null;
  brushCloseup.hidden = !brush;
  room.classList.toggle('is-brush-closeup', Boolean(brush));
  if (brush) {
    const frame = Math.floor(brush.time * 4) % 2;
    const path = `assets/luon/brush-closeup-v1/${wearingBackpack ? 'bag' : 'plain'}_${frame}.svg`;
    if (brushCharacter.getAttribute('src') !== path) brushCharacter.setAttribute('src', path);
    brushCloseup.style.opacity = String(Math.min(1, brush.time / .3, (3.2 - brush.time) / .3));
  }
  luon.style.left = `${(onBalcony ? journeyDisplayX(position.x) : position.x) * 100}%`;
  if (onBalcony) drawJourneyMap();
  luon.style.top = `${position.y * 100}%`;
  luon.classList.toggle('washroom-sink', journeyZone === 'washroom' && Math.abs(position.x - .82) <= journeyReach());
  luon.classList.toggle('gesture', Boolean(actionSession?.journeyAction) || (actionSession?.kind === 'door' && actionSession.phase === 'acting'));
  luon.classList.toggle('flipped', !seat && !tabletSession && !actionSession?.journeyAction && (!actionSession || actionSession.phase === 'balcony') && facingLeft);
  luon.classList.toggle('sleeping', ['lying-down', 'sleeping', 'getting-up'].includes(actionSession?.phase));
  luon.classList.toggle('sleeping-right', luon.classList.contains('sleeping') && actionSession.id.endsWith('right'));
  luon.classList.toggle('climbing', actionSession?.kind === 'climb' && ['acting', 'descending'].includes(actionSession.phase));
  luon.classList.toggle('seated', Boolean(seat));
  const slippersVisible = actionSession?.kind === 'climb' && !['removing-slippers', 'putting-slippers'].includes(actionSession.phase);
  document.querySelector('#sleep-slippers-left').hidden = !slippersVisible || !actionSession.id.endsWith('left');
  document.querySelector('#sleep-slippers-right').hidden = !slippersVisible || !actionSession.id.endsWith('right');
  const coveredBackpack = actionSession?.kind === 'backpack' ? actionSession.id : wearingBackpack ? carriedBackpackId : null;
  document.querySelector('#backpack-floor-left').hidden = coveredBackpack !== 'backpack_left';
  document.querySelector('#backpack-floor-right').hidden = coveredBackpack !== 'backpack_right';
}
function resetCheckpoint(target = spawn, options = {}) {
  keys.clear();
  deskGame = null; deskPanel.hidden = true; sessionStats.minutes = 8 * 60; sessionStats.study = 0; sessionStats.fun = 0; renderStats();
  seat = null; tabletSession = null; actionSession = null; onBalcony = false; journeyZone = null; journeyScene.hidden = true; brushed = false; refreshed = false; roomLight = true; lampLights.left = true; lampLights.right = true; updateLights(); wearingBackpack = Boolean(options.backpack);
  carriedBackpackId = wearingBackpack ? 'backpack_left' : null; studying = false;
  position.x = target.x; position.y = target.y;
  nearby = null; interactionHint.hidden = true;
  facingLeft = false; walkingTime = 0; lastFrame = 0;
  setSprite(idleSprite());
  if (selected) { selected.classList.remove('selected'); selected.setAttribute('aria-pressed', 'false'); selected = null; }
  details.innerHTML = '<div class="empty-icon">✳</div><h2>Nothing selected</h2><p>Choose an object in the room to see its details.</p>';
  mode = 'off';
  document.querySelectorAll('.mode').forEach(button => { const active = button.dataset.mode === 'off'; button.classList.toggle('active', active); button.setAttribute('aria-pressed', String(active)); });
  updateReference();
  checkpoint.hidden = true;
  if (options.seated) {
    const item = interactables.find(entry => entry.object.id === options.seated);
    if (item) { sitDown(item); if (options.deskGame) startDeskGame(options.deskGame); }
  }
  if (options.roomLightOff) roomLight = false;
  if (options.lampOff) lampLights[options.lampOff] = false;
  if (options.sleeping) {
    const item = interactables.find(entry => entry.object.id === `ladder_${options.sleeping}`);
    if (item) {
      startAction('climb', item);
      actionSession.top = { x: position.x, y: 0.33 };
      actionSession.phase = 'sleeping';
      actionSession.time = 0;
      Object.assign(position, actionSession.destination);
      setSprite(sleepSprite(0));
    }
  }
  if (options.balcony) {
    actionSession = { kind: 'door', id: 'door_main', previous: { ...actionAnchors.door_main }, time: 0, phase: 'balcony' };
    setJourneyZone(options.zone || 'hallway', options.zone === 'washroom' ? .135 : options.zone === 'lobby' ? .87 : options.zone === 'bus' ? .15 : .606);
    brushed = Boolean(options.brushed); refreshed = Boolean(options.refreshed);
    if (options.journeyAction) {
      const targetId = ['brush', 'wash'].includes(options.journeyAction) ? 'sink' : options.journeyAction;
      const point = journeyZones[journeyZone].points.find(entry => entry.id === targetId);
      if (point) { position.x = point.x; startJourneyAction(options.journeyAction); }
    }
  }
  updateLights();
  drawLuon();
  updateNearby();
}
function updateNearby() {
  if (actionSession) {
    nearby = null;
    interactionHint.hidden = !checkpoint.hidden || !['sleeping', 'balcony'].includes(actionSession.phase);
    if (actionSession.journeyAction) { interactionHint.hidden = true; return; }
    if (onBalcony) {
      const point = nearbyJourneyPoint();
      const label = point?.id === 'sink' ? (!brushed || refreshed ? 'Đánh răng' : 'Rửa mặt') : point?.label;
      interactionHint.textContent = point && Math.abs(position.x - point.x) <= journeyReach() ? `E · ${label}   Q · Quay lại` : 'A / D · Di chuyển   Q · Quay lại';
    } else interactionHint.textContent = 'Q · Xuống giường';
    return;
  }
  if (tabletSession) {
    nearby = null;
    interactionHint.hidden = !checkpoint.hidden;
    interactionHint.textContent = 'E · Cất tablet   Q · Kết thúc';
    return;
  }
  if (seat) {
    nearby = null;
    interactionHint.hidden = !checkpoint.hidden;
    interactionHint.textContent = deskGame?.phase === 'playing' ? 'Space · Canh thời điểm' : deskGame?.phase === 'finishing' ? 'Đang hoàn thành…' : 'Chọn hoạt động trên màn hình · Q · Đứng dậy';
    return;
  }
  let distance = 70;
  nearby = null;
  for (const item of interactables) {
    const object = item.object;
    if (wearingBackpack && object.id.startsWith('backpack_') && object.id !== carriedBackpackId) continue;
    const anchorX = actionAnchors[object.id]?.x ?? object.x + object.width / 2;
    const anchorY = actionAnchors[object.id]?.y ?? object.y + object.height;
    if (object.id.startsWith('bed_')) continue;
    const candidate = Math.hypot((position.x - anchorX) * 768, (position.y - anchorY) * 512);
    if (candidate < distance) { distance = candidate; nearby = item; }
  }
  interactionHint.hidden = !nearby || !checkpoint.hidden;
  if (nearby && checkpoint.hidden) {
    const id = nearby.object.id;
    const action = id.startsWith('backpack_') && wearingBackpack ? 'Bỏ ba lô' : id.startsWith('ladder_') ? 'Leo lên giường' : id === 'light_switch' ? (roomLight ? 'Tắt đèn phòng' : 'Bật đèn phòng') : id.startsWith('lamp_') ? (lampLights[id.endsWith('left') ? 'left' : 'right'] ? 'Tắt đèn bàn' : 'Bật đèn bàn') : id === 'door_main' ? 'Mở cửa ra hành lang' : nearby.object.action;
    interactionHint.textContent = `E · ${nearby.object.name} (${action})`;
  }
}
function sitDown(item) {
  const object = item.object;
  seat = { previous: { ...position }, id: object.id };
  studying = false; keys.clear();
  position.x = object.x + object.width / 2;
  position.y = object.y + object.height - 0.01;
  setSprite(seatedSprite(0));
  drawLuon(); showDeskMenu();
}
function standUp() {
  position.x = seat.previous.x; position.y = seat.previous.y;
  seat = null; studying = false; deskGame = null; deskPanel.hidden = true; keys.clear();
  setSprite(idleSprite());
  drawLuon(); updateNearby();
}
function animateLuon(now) {
  const dt = lastFrame ? Math.min((now - lastFrame) / 1000, 0.05) : 0;
  lastFrame = now;
  if (actionSession) {
    const action = actionSession;
    action.time += dt;
    if (action.kind === 'climb') {
      if (action.phase === 'removing-slippers' || action.phase === 'putting-slippers') {
        const putting = action.phase === 'putting-slippers';
        const frame = action.time < .45 ? (putting ? 1 : 0) : (putting ? 0 : 1);
        setSprite(`assets/luon/slippers-action-v1/${wearingBackpack ? 'bag' : 'plain'}_${frame}.svg`);
        if (action.time >= .9) {
          if (putting) finishAction();
          else { action.phase = 'acting'; action.time = 0; }
        }
      } else if (action.phase === 'acting') {
        const t = Math.min(action.time / 1.6, 1);
        position.y = 0.57 + (0.33 - 0.57) * t;
        setSprite(climbSprite(Math.floor(action.time * 4) % 4));
        if (t === 1) { action.phase = 'lying-down'; action.time = 0; action.top = { ...position }; }
      } else if (action.phase === 'lying-down') {
        const t = Math.min(action.time / 0.45, 1);
        position.x = action.top.x + (action.destination.x - action.top.x) * t;
        position.y = action.top.y + (action.destination.y - action.top.y) * t;
        setSprite(sleepSprite(0));
        if (t === 1) { action.phase = 'sleeping'; action.time = 0; }
      } else if (action.phase === 'getting-up') {
        const t = Math.min(action.time / 0.45, 1);
        position.x = action.destination.x + (action.top.x - action.destination.x) * t;
        position.y = action.destination.y + (action.top.y - action.destination.y) * t;
        setSprite(sleepSprite(0));
        if (t === 1) { action.phase = 'descending'; action.time = 0; }
      } else if (action.phase === 'descending') {
        const t = Math.min(action.time / 1.6, 1);
        position.y = 0.33 + (0.57 - 0.33) * t;
        // Replay the ascent frames backwards, starting at its final pose.
        const reverseTime = Math.max(0, 1.6 - action.time);
        setSprite(climbSprite(Math.floor(reverseTime * 4) % 4));
        if (t === 1) { action.phase = 'putting-slippers'; action.time = 0; }
      } else setSprite(sleepSprite(Math.floor(action.time / 2) % 2));
    } else if (action.phase === 'entering') {
      const t = Math.min(action.time / 0.7, 1);
      setSprite(doorSprite(1));
      if (t === 1) { action.phase = 'crossing'; action.time = 0; keys.clear(); }
    } else if (action.phase === 'crossing') {
      const t = Math.min(action.time / 0.6, 1);
      position.x = action.previous.x + (0.63 - action.previous.x) * t;
      position.y = action.previous.y + (0.34 - action.previous.y) * t;
      const frames = wearingBackpack ? backpackWalkFrames : walkFrames;
      setSprite(frames[Math.floor(action.time * 6) % frames.length]);
      if (t === 1) setJourneyZone('hallway', .606, 'Đã ra hành lang tầng 4.');
    } else if (action.kind === 'backpack') {
      const variant = action.pickingUp ? 'pickup' : 'putdown';
      const frame = action.time < 0.6 ? 0 : 1;
      setSprite(`assets/luon/backpack-action-v1/${variant}_${frame}.svg`);
      if (action.time >= 1.2) {
        wearingBackpack = action.pickingUp;
        carriedBackpackId = wearingBackpack ? action.id : null;
        actionSession = null;
        setSprite(idleSprite());
      }
    } else if (action.phase === 'acting') {
      const frame = Math.floor(action.time * 4) % 2;
      if (action.kind === 'door') setSprite(doorSprite(0));
      else setSprite(`assets/luon/lights-v1/${wearingBackpack ? 'bag' : 'plain'}_${action.kind === 'lamp' ? 'lamp' : 'switch'}_${frame}.svg`);
      if (action.time >= 0.8) {
        if (action.kind === 'light') roomLight = !roomLight;
        if (action.kind === 'lamp') { const side = action.id.endsWith('left') ? 'left' : 'right'; lampLights[side] = !lampLights[side]; }
        if (action.kind === 'door') { action.phase = 'entering'; action.time = 0; }
        else { actionSession = null; setSprite(idleSprite()); }
        updateLights();
      }
    } else if (action.journeyAction) {
      const current = action.journeyAction;
      if (checkpoint.hidden) current.time += dt;
      if (current.kind === 'exit-wc') setSprite(doorSprite(current.time < .6 ? 0 : 1));
      else if (current.kind === 'brush' || current.kind === 'wash') {
        const frame = Math.floor(current.time * 4) % 2;
        setSprite(`assets/luon/washroom-v2/${wearingBackpack ? 'bag' : 'plain'}_${current.kind}_${frame}.svg`);
      } else {
        const direction = current.kind === 'stairs' ? 'down' : 'up';
        const frame = Math.floor(current.time * 4) % 2;
        setSprite(`assets/luon/stairs-v1/${wearingBackpack ? 'bag' : 'plain'}_${direction}_${frame}.svg`);
        const t = Math.min(current.time / 2.2, 1);
        const descending = current.kind === 'stairs';
        position.x = descending ? .12 + .56 * t : .35 + .48 * t;
        position.y = descending ? .34 + .51 * t : .85 - .52 * t;
      }
      const duration = ['stairs', 'upstairs'].includes(current.kind) ? 2.2 : current.kind === 'brush' ? 3.2 : 1.5;
      if (current.time >= duration) {
        action.journeyAction = null;
        if (current.kind === 'brush' || current.kind === 'wash') position.y = current.previousY;
        if (current.kind === 'brush') { brushed = true; refreshed = false; journeyNotice.textContent = 'Đã đánh răng. Tiếp theo: rửa mặt.'; }
        else if (current.kind === 'wash') { refreshed = true; journeyNotice.textContent = 'Đã rửa mặt. Có thể ra hành lang.'; }
        else if (current.kind === 'stairs') setJourneyZone('lobby', .87, 'Đã xuống sảnh KTX.');
        else if (current.kind === 'exit-wc') setJourneyZone('hallway', .259, 'Đã ra hành lang.');
        else setJourneyZone('hallway', .91, 'Đã lên hành lang tầng 4.');
      }
    } else {
      let x = Number(keys.has('d')) - Number(keys.has('a'));
      let y = journeyZone === 'hallway' ? 0 : Number(keys.has('s')) - Number(keys.has('w'));
      if (x || y) {
        const length = Math.hypot(x, y); x /= length; y /= length;
        const hall = journeyZone === 'hallway';
        position.x = Math.max(hall ? 0.04 : 0.08, Math.min(hall ? 0.96 : 0.92, position.x + x * (hall ? 0.66 / hallwayScale : 0.66) * dt));
        position.y = hall ? 0.88 : journeyZone === 'washroom' ? Math.max(0.84, Math.min(0.92, position.y + y * 0.99 * dt)) : Math.max(0.68, Math.min(0.94, position.y + y * 0.99 * dt));
        if (x) facingLeft = x < 0;
        walkingTime += dt;
        const frames = wearingBackpack ? backpackWalkFrames : walkFrames;
        setSprite(frames[Math.floor(walkingTime * 18) % frames.length]);
      } else { walkingTime = 0; setSprite(idleSprite()); }
    }
    drawLuon(); updateNearby(); requestAnimationFrame(animateLuon); return;
  }
  if (tabletSession) {
    walkingTime += dt;
    setSprite(tabletSprite(Math.floor(walkingTime * 2) % 4));
    drawLuon(); updateNearby(); requestAnimationFrame(animateLuon); return;
  }
  if (seat) {
    tickDeskGame(dt);
    if (studying && checkpoint.hidden) walkingTime += dt;
    setSprite(studying ? seatedSprite(Math.floor(walkingTime * (deskGame?.kind === 'game' ? 4 : 2)) % 4) : seatedSprite(0));
    drawLuon();
    updateNearby();
    requestAnimationFrame(animateLuon);
    return;
  }
  let x = Number(keys.has('d')) - Number(keys.has('a'));
  let y = Number(keys.has('s')) - Number(keys.has('w'));
  if (x || y) {
    const length = Math.hypot(x, y); x /= length; y /= length;
    const previousX = position.x, previousY = position.y;
    position.x = Math.max(0.06, Math.min(0.94, position.x + x * 0.66 * dt));
    position.y = Math.max(0.40, Math.min(0.91, position.y + y * 0.99 * dt));
    if (x) facingLeft = x < 0;
    const moved = position.x !== previousX || position.y !== previousY;
    if (moved) walkingTime += dt;
    const frames = wearingBackpack ? backpackWalkFrames : walkFrames;
    setSprite(moved ? frames[Math.floor(walkingTime * 18) % frames.length] : idleSprite());
  } else {
    walkingTime = 0;
    setSprite(idleSprite());
  }
  drawLuon();
  updateNearby();
  requestAnimationFrame(animateLuon);
}
window.addEventListener('keydown', event => {
  if (event.key === 'F8') { event.preventDefault(); keys.clear(); checkpoint.hidden = !checkpoint.hidden; return; }
  if (!checkpoint.hidden) {
    const target = { '1': spawn, '2': { x: 0.26, y: 0.60 }, '3': { x: 0.63, y: 0.42 }, '4': { x: 0.139, y: 0.64 }, '5': { x: 0.861, y: 0.64 }, '6': { x: 0.09, y: 0.77 }, '7': { x: 0.139, y: 0.64 }, '8': { x: 0.53, y: 0.91 } }[event.key];
    if (target) { event.preventDefault(); resetCheckpoint(target, event.key === '7' ? { backpack: true, seated: 'chair_left', deskGame: 'study' } : {}); }
    return;
  }
  if (event.target instanceof HTMLElement && /^(INPUT|TEXTAREA|SELECT)$/.test(event.target.tagName)) return;
  const key = event.key.toLowerCase();
  if (actionSession?.journeyAction) return;
  if ((key === 'q' || key === 'e') && !event.repeat && actionSession && (actionSession.phase === 'sleeping' || (actionSession.phase === 'balcony' && key === 'q'))) {
    event.preventDefault();
    if (actionSession.phase === 'sleeping') { actionSession.phase = 'getting-up'; actionSession.time = 0; }
    else leaveJourneyZone();
    return;
  }
  if (actionSession && actionSession.phase !== 'balcony') return;
  if (actionSession && actionSession.phase === 'balcony') {
    if (key === 'e' && !event.repeat) { event.preventDefault(); activateJourneyPoint(nearbyJourneyPoint()); return; }
    if ('wasd'.includes(key) && key.length === 1) { event.preventDefault(); keys.add(key); }
    return;
  }
  if ((key === 'q' || key === 'e') && !event.repeat && tabletSession) {
    event.preventDefault();
    position.x = tabletSession.previous.x; position.y = tabletSession.previous.y;
    tabletSession = null; keys.clear(); drawLuon(); updateNearby();
    return;
  }
  if (seat && deskGame?.phase === 'playing' && (event.code === 'Space' || key === 'e') && !event.repeat) {
    if (event.code === 'Space' && event.target === document.querySelector('#timing-skip')) return;
    event.preventDefault(); hitTiming(); return;
  }
  if (seat && deskGame?.phase === 'result' && (key === 'e' || event.code === 'Space') && !event.repeat) { event.preventDefault(); showDeskMenu(); return; }
  if (key === 'q' && !event.repeat && seat) { event.preventDefault(); if (!deskGame || deskGame.phase === 'result') standUp(); return; }
  if (key === 'e' && !event.repeat) {
    if (seat) {
      event.preventDefault();
      if (!deskGame) showDeskMenu();
      return;
    }
    if (nearby) {
      event.preventDefault();
      const item = nearby;
      selectObject(item.object, item.element);
      if (item.object.id.startsWith('ladder_')) startAction('climb', item);
      else if (item.object.id === 'light_switch') startAction('light', item);
      else if (item.object.id.startsWith('lamp_')) startAction('lamp', item);
      else if (item.object.id === 'door_main') startAction('door', item);
      else if (item.object.action === 'Sit') sitDown(item);
      else if (item.object.id === 'tablet_center') {
        tabletSession = { previous: { ...position } }; keys.clear();
        walkingTime = 0;
        setSprite(tabletSprite());
        drawLuon(); updateNearby();
      }
      else if (item.object.id.startsWith('backpack_')) {
        if (wearingBackpack && item.object.id !== carriedBackpackId) return;
        startAction('backpack', item);
        drawLuon(); updateNearby();
      }
      return;
    }
  }
  if ('wasd'.includes(key) && key.length === 1) { event.preventDefault(); keys.add(key); }
});
window.addEventListener('keyup', event => keys.delete(event.key.toLowerCase()));
window.addEventListener('blur', () => keys.clear());
document.addEventListener('visibilitychange', () => { if (document.hidden) keys.clear(); });
document.querySelector('#wash-brush').addEventListener('click', () => {
  if (journeyZone === 'washroom' && Math.abs(position.x - .82) <= journeyReach()) startJourneyAction('brush');
});
washFaceButton.addEventListener('click', () => {
  if (journeyZone === 'washroom' && brushed && Math.abs(position.x - .82) <= journeyReach()) startJourneyAction('wash');
});
document.querySelector('#choose-study').addEventListener('click', () => startDeskGame('study'));
document.querySelector('#choose-game').addEventListener('click', () => startDeskGame('game'));
document.querySelector('#timing-hit').addEventListener('click', hitTiming);
document.querySelector('#timing-skip').addEventListener('click', () => finishDeskGame(true));
document.querySelector('#result-continue').addEventListener('click', showDeskMenu);
for (const [id, options] of [
  ['checkpoint-desk-menu', { seated: 'chair_left' }],
  ['checkpoint-desk-timing', { seated: 'chair_left', deskGame: 'study' }],
  ['checkpoint-desk-game', { seated: 'chair_left', deskGame: 'game' }]
]) document.querySelector(`#${id}`).addEventListener('click', () => resetCheckpoint({ x: 0.139, y: 0.64 }, options));
renderStats();
document.querySelector('#checkpoint-center').addEventListener('click', () => resetCheckpoint(spawn));
document.querySelector('#checkpoint-ladder').addEventListener('click', () => resetCheckpoint({ x: 0.26, y: 0.60 }));
document.querySelector('#checkpoint-door').addEventListener('click', () => resetCheckpoint({ x: 0.63, y: 0.42 }));
document.querySelector('#checkpoint-chair-left').addEventListener('click', () => resetCheckpoint({ x: 0.139, y: 0.64 }));
document.querySelector('#checkpoint-chair-right').addEventListener('click', () => resetCheckpoint({ x: 0.861, y: 0.64 }));
document.querySelector('#checkpoint-backpack').addEventListener('click', () => resetCheckpoint({ x: 0.09, y: 0.77 }));
document.querySelector('#checkpoint-study').addEventListener('click', () => resetCheckpoint({ x: 0.139, y: 0.64 }, { backpack: true, seated: 'chair_left', deskGame: 'study' }));
document.querySelector('#checkpoint-tablet').addEventListener('click', () => resetCheckpoint({ x: 0.53, y: 0.91 }));

for (const [id, point] of Object.entries({ 'checkpoint-light': actionAnchors.light_switch, 'checkpoint-lamp': actionAnchors.lamp_left, 'checkpoint-lamp-right': actionAnchors.lamp_right, 'checkpoint-climb': actionAnchors.ladder_left, 'checkpoint-climb-right': actionAnchors.ladder_right, 'checkpoint-balcony': actionAnchors.door_main })) {
  document.querySelector(`#${id}`).addEventListener('click', () => resetCheckpoint(point));
}
for (const [id, target, options] of [
  ['checkpoint-sleep-left', actionAnchors.ladder_left, { sleeping: 'left' }],
  ['checkpoint-sleep-right', actionAnchors.ladder_right, { sleeping: 'right' }],
  ['checkpoint-light-off', actionAnchors.light_switch, { roomLightOff: true }],
  ['checkpoint-lamp-off', actionAnchors.lamp_left, { lampOff: 'left' }],
  ['checkpoint-balcony-inside', actionAnchors.door_main, { balcony: true }],
  ['checkpoint-washroom', actionAnchors.door_main, { balcony: true, zone: 'washroom' }],
  ['checkpoint-washroom-face', actionAnchors.door_main, { balcony: true, zone: 'washroom', brushed: true }],
  ['checkpoint-brush-action', actionAnchors.door_main, { balcony: true, zone: 'washroom', journeyAction: 'brush' }],
  ['checkpoint-wash-action', actionAnchors.door_main, { balcony: true, zone: 'washroom', brushed: true, journeyAction: 'wash' }],
  ['checkpoint-stairs-action', actionAnchors.door_main, { balcony: true, zone: 'hallway', journeyAction: 'stairs' }],
  ['checkpoint-upstairs-action', actionAnchors.door_main, { balcony: true, zone: 'lobby', journeyAction: 'upstairs' }],
  ['checkpoint-wc-exit', actionAnchors.door_main, { balcony: true, zone: 'washroom' }],
  ['checkpoint-lobby', actionAnchors.door_main, { balcony: true, zone: 'lobby' }],
  ['checkpoint-bus-stop', actionAnchors.door_main, { balcony: true, zone: 'bus' }],
  ['checkpoint-walk-bag', spawn, { backpack: true }],
  ['checkpoint-tablet-bag', { x: .53, y: .91 }, { backpack: true }],
  ['checkpoint-backpack-putdown', { x: .09, y: .77 }, { backpack: true }],
  ['checkpoint-climb-bag', actionAnchors.ladder_left, { backpack: true }],
  ['checkpoint-door-bag', actionAnchors.door_main, { backpack: true }],
  ['checkpoint-lamp-bag', actionAnchors.lamp_left, { backpack: true }],
  ['checkpoint-light-bag', actionAnchors.light_switch, { backpack: true }]
]) document.querySelector(`#${id}`).addEventListener('click', () => resetCheckpoint(target, options));
document.querySelector('#checkpoint-close').addEventListener('click', () => { checkpoint.hidden = true; });
drawLuon();
requestAnimationFrame(animateLuon);

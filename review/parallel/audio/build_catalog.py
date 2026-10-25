"""Rebuild owned audio catalog from existing production event IDs; no Unity/editor calls."""
import json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[3]
out=ROOT/'mobile-2d/Assets/Resources/ProductionAudio/catalog.json'
cues=[]
def cue(id,group,source,duration=0.3,volume=.4,loop=False,status=None):
    cues.append(dict(id=id,group=group,source=source,rights='Original project procedural synthesis; no external samples, no third-party recording license.',duration=duration,volume=volume,loop=loop,status=status or ('existing_procedural' if source.startswith('DormSound') else 'procedural_placeholder')))
for id,group,method,loop,vol in [
 ('water','A_AMBIENCE','Water',True,.28),('typing','A_STUDY','Typing',True,.28),('writing','A_STUDY','Writing',True,.32),('footsteps','A_TRAVEL','Footsteps',True,.35),('bus_engine','A_TRAVEL','BusEngine',True,.3),('hotpot','A_FRIENDS','Hotpot',True,.3),('glass_clink','A_FRIENDS','GlassClink',False,.4)]:
    cue(id,group,'DormSound.Create'+method,1,vol,loop)
for id,kind in [('station','crowd'),('dorm','fan'),('classroom','hum'),('canteen','crowd'),('lab','hum'),('street','wind'),('lake','wind'),('ceremony','crowd'),('binh_dong','wind')]:
    cue('amb_'+id,'A_AMBIENCE','synth:'+kind,4,.25,True)
for id,group,kind,duration,vol in [
 ('step','A_TRAVEL','door',.16,.18),('stairs_step','A_TRAVEL','door',.18,.22),('bag','A_TRAVEL','cloth',.55,.45),('door','A_TRAVEL','door',.65,.4),('key','A_TRAVEL','keys',.4,.3),('bus_arrival','A_TRAVEL','hum',1.2,.3),
 ('paper','A_STUDY','cloth',.35,.3),('click','A_STUDY','click',.08,.28),('compile','A_STUDY','click',.18,.3),('test_pass','A_STUDY','success',.4,.35),('error','A_STUDY','error',.35,.35),('submit','A_STUDY','success',.45,.35),
 ('fan_switch','A_LIFE','click',.12,.3),('wipe','A_LIFE','cloth',.65,.4),('alarm','A_LIFE','alarm',2,.35),('snooze','A_LIFE','click',.15,.3),('phone_vibrate','A_LIFE','phone',1.2,.35),('phone_accept','A_LIFE','click',.15,.3),('phone_end','A_LIFE','error',.2,.2),('money','A_LIFE','cloth',.3,.35),('usb','A_LIFE','click',.15,.3),('screwdriver','A_LIFE','keys',.6,.25),
 ('guard_keys','A_STEALTH','keys',.9,.4),('unplug','A_STEALTH','click',.15,.35),('lift_pot','A_STEALTH','door',.3,.25),('cupboard','A_STEALTH','door',.5,.3),('timer','A_STEALTH','click',.08,.25),
 ('knock','A_FRIENDS','knock',.8,.4),('food_package','A_FRIENDS','cloth',.55,.4),('camera','A_FRIENDS','click',.18,.45),('greeting_cloth','A_FRIENDS','cloth',.7,.35),
 ('printer','A_THESIS_END','printer',2,.35),('projector','A_THESIS_END','hum',1,.3),('applause','A_THESIS_END','applause',2,.3),('degree_paper','A_THESIS_END','cloth',.5,.35),('light_switch','A_THESIS_END','click',.12,.4),('final_door','A_THESIS_END','door',.8,.35),
 ('ui_hover','A_UI','click',.05,.15),('ui_confirm','A_UI','click',.1,.3),('ui_cancel','A_UI','error',.12,.2),('ui_error','A_UI','error',.3,.3),('ui_complete','A_UI','success',.45,.35),('ui_reward','A_UI','success',.5,.35),('ui_lock','A_UI','click',.15,.25),('ui_unlock','A_UI','success',.25,.3)]:
    cue(id,group,'synth:'+kind,duration,vol)
# Semantic variants of original existing sound source, not new recorded assets.
cue('patrol_step','A_STEALTH','synth:door',.2,.22)
for name in ['student','gaming','decline','livelihood','lake','farewell','recovery','thesis','defense','graduation','acoustic_credits']:
    cue('bgm_'+name,'A_BGM','pending',1,.5,True,'pending_music')
# Only short one-shots on commits. Continuous audio is driven by physical start/stop APIs.
exact={
 'hear_guard':'guard_keys','unplug_cooker':'unplug','hide_pot':'cupboard','pretend_sleep':'greeting_cloth',
 'answer_mother':'phone_accept','call_mother':'phone_accept','finish_call':'phone_end','choose_alarm':'ui_confirm',
 'wake_immediately':'snooze','set_alarm':'ui_confirm','hear_knock':'knock','toast':'glass_clink',
 'repair_fan':'screwdriver','clean_four_spots':'wipe','plug_cooker':'fan_switch','get_cooker':'lift_pot',
 'add_ingredients':'food_package','share_feast':'food_package','share_hotpot':'food_package','share_tea':'glass_clink',
 'take_group_photo':'camera','print_report':'printer','connect_projector':'projector','receive_grade_a':'applause',
 'receive_degree':'degree_paper','turn_off_light':'light_switch','close_room_door':'final_door','return_key':'key',
 'unlock_room':'key','receive_room_key':'key','boot_usb':'usb','finish_credits':'ui_complete',
 'read_failed_courses':'ui_error','read_data_structures_fail':'ui_error','read_rejection':'ui_error',
 'receive_laptop':'bag','receive_crush_laptop':'bag','handover_laptop':'bag','take_book':'paper','deliver_book':'paper',
 'receive_to_gift':'food_package','receive_supplies':'food_package','open_for_friends':'door',
 'hold_uninstall':'ui_complete','select_uninstall':'ui_confirm','read_debt_cleared':'ui_complete',
 'confirm_136_credits':'ui_complete','confirm_schedule':'ui_complete','repair_crush_laptop':'screwdriver',
 'repair_second_laptop':'screwdriver','receive_job_email':'phone_vibrate','receive_first_job':'phone_vibrate'}
rules=[(('compile','run_first_demo','present_demo'), 'compile'),(('error','crash','failed'), 'error'),(('test','validate','pass_'), 'test_pass'),(('submit','send_','export_'),'submit'),(('patch','implement','edit_','format_','scan_','install_','backup_'),'click'),(('study','revise','write_','answer_known','finish_lesson'),'paper'),(('buy_','pay_','waive_','ledger'),'money'),(('backpack','pack_','boxes','jacket','shirt'),'bag'),(('say_goodbye','link_shoulders','reunite','meet_family'),'greeting_cloth'),(('bus','travel_','arrive_dorm'),'bus_arrival'),(('receive_','message','invite','notice'),'phone_vibrate'),(('read_','open_','select_','choose_','review_','accept_','register_','enroll_','book_','apply_','refresh_'),'ui_confirm')]
def select(step):
    if step in exact: return exact[step]
    for terms,chosen in rules:
        if any(term in step for term in terms): return chosen
    return 'ui_confirm'
data=json.loads((ROOT/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json').read_text())
events=[dict(mission=m['id'],step=s['id'],cue=select(s['id'])) for m in data['missions'] for s in m['steps']]
scenes=[]
for id,amb,music in [('Station','station','student'),('Reception','dorm','student'),('RoomDay','dorm','student'),('RoomNight','dorm','gaming'),('Restroom',None,None),('Classroom','classroom','recovery'),('Canteen','canteen','student'),('Lab','lab','thesis'),('Street','street','livelihood'),('Lake','lake','lake'),('Farewell','dorm','farewell'),('Recovery','dorm','recovery'),('Thesis','lab','thesis'),('Defense','classroom','defense'),('Ceremony','ceremony','graduation'),('BinhDong','binh_dong','graduation'),('Credits',None,'acoustic_credits'),('GradesSilence',None,None),('PhoneSilence',None,None)]:
    scenes.append(dict(id=id,ambience='amb_'+amb if amb else '',music='bgm_'+music if music else ''))
out.write_text(json.dumps(dict(version=1,cues=cues,events=events,scenes=scenes),ensure_ascii=False,indent=2)+'\n')
print(f'{len(cues)} cues; {len(events)} event mappings; {len(scenes)} scene profiles')

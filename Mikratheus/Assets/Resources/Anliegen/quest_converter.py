import os

TEMPLATE = """
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!114 &11400000
MonoBehaviour:
  m_ObjectHideFlags: 0
  m_CorrespondingSourceObject: {{fileID: 0}}
  m_PrefabInstance: {{fileID: 0}}
  m_PrefabAsset: {{fileID: 0}}
  m_GameObject: {{fileID: 0}}
  m_Enabled: 1
  m_EditorHideFlags: 0
  m_Script: {{fileID: 11500000, guid: 01f32daaea4cd3e488effe414c4c2517, type: 3}}
  m_Name: {0}
  m_EditorClassIdentifier: 
  sprite: {{fileID: 21300000, guid: 31ad314fb4577bf4bb5fd52757498fc9, type: 3}}
  message: "{1}"
  question: "{2}"
  sender: "From: {3}"
  subject: "Subject: {4}"
  timelimitsec: {5}
  approveButtonText: "{6}"
  approveCost: {7}
  approveFollowerMod: {8}
  approveInfluence: {9}
  denyButtonText: "{10}"
  denyCost: {11}
  denyFollowerMod: {12}
  denyInfluence: {13}
  ignoreFollowerMod: {14}
  ignoreInfluence: {15}
"""

data = []

def to_int(gp):

    sign = 1
    if gp[0] == '-':
        sign = -1
        
    return sign * int(gp[1:-2])


for name in ('konteos','borea','eco_827','nobola'):

    data = ""

    with open(name + '.txt','r') as f:
        data = f.read().strip()
        
    
    tasks = data.split('---------------------------')
    
    os.mkdir(name + '\\start')
    
    for nr, task in enumerate(tasks):
    
    
        # clean up
    
        task = task.strip()
        
        task_lines = task.split("\n")
        
        to_remove = []
        
        for line in task_lines:
            line = line.strip()
            
            if len(line) > 0 and line[0] == '(':
                to_remove.append(line)
             
        for line in to_remove:
            task_lines.remove(line)
            
            
        while '' in task_lines:
            task_lines.remove('')
        
        
        # parse
        
        quest_name        = name + str(nr)
        
        appr_fol_mod      =  100
        deny_fol_mod      = -100
        ign_fol_mod       = -50
        
        time_limit_sec    = 60
        
        
        dir_suffix = ""
        
        if "Revival" in task_lines[1]:
            dir_suffix = "\\start"
        
        
        from_             = task_lines[2][5:]
        subject           = task_lines[3][9:]
        
        message           = ' '.join( task_lines[i] for i in range (6, task_lines.index('QUESTION')))
        
        question          = task_lines[ task_lines.index('QUESTION') + 1]
        
        
        appr_text         = ""

        indx = task_lines.index('YES') + 1
        
        while task_lines[indx][0] not in ('-','+'):
            appr_text += task_lines[indx]
            indx += 1
        
        
        appr_cost         = to_int(task_lines[ task_lines.index('NO') - 2])
        appr_infl         = to_int(task_lines[ task_lines.index('NO') - 1])


        deny_text         = ""

        indx = task_lines.index('NO') + 1
        
        while task_lines[indx][0] not in ('-','+'):
            deny_text += task_lines[indx]
            indx += 1
        
        
        deny_cost         = to_int(task_lines[indx])
        deny_infl         = to_int(task_lines[indx + 1])

        ign_infl          = to_int(task_lines[indx + 3])
        
        
        
        with open(name + dir_suffix + "\\" + quest_name + '.asset','w') as f:
            f.write(TEMPLATE.format(
                quest_name,
                  message.replace('"', "'"),
                  question.replace('"', "'"),
                  from_,
                  subject,
                  time_limit_sec,
                  appr_text.replace('"', "'"),
                  appr_cost,
                  appr_fol_mod,
                  appr_infl,
                  deny_text.replace('"', "'"),
                  deny_cost,
                  deny_fol_mod,
                  deny_infl,
                  ign_fol_mod,
                  ign_infl
            ).strip())
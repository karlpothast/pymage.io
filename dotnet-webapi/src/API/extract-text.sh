#!/usr/bin/env bash

set -Euo pipefail
#set e #fail immediately on error
trap cleanup SIGINT SIGTERM ERR EXIT

# script_dir=$(cd "$(dirname "${BASH_SOURCE[0]}")" &>/dev/null && pwd -P)

help() {
  cat << EOF

Usage: $(basename "${BASH_SOURCE[0]}") [-h] -b -u [-v]

Execute base64 to file command

Available options:
------------------------------------------------
-b  --base64string          base64 string
-u  --blobURL               blob URL 
-h  --help                  help menu
-v  --verbose               verbose output

EOF
  exit
}

show_debug_info() {
  msg "Script debug info :"
  msg "-------------------"
  msg "Parameters read: :"
  msg "- b: ${base64string}"
  msg "- u: ${blobURL}"
}

cleanup() {
  trap - SIGINT SIGTERM ERR EXIT
}

msg() {
  echo >&2 -e "${1-}"
}

quit() {
  local msg=$1
  local code=${2-1} # default exit status 1
  msg "$msg"
  exit "$code"
}

parse_params() {
  base64string=''
  blobURL=''

  while :; do
    case "${1-}" in
    -h | --help) help ;;
    -v | --verbose) set -x ;;
    -b | --base64string)
      base64string="${2-}"
      shift
      ;;
    -u | --blobURL)
      blobURL="${2-}"
      shift
      ;;
    -?*) quit "Unknown option: $1" ;;
    *) break ;;
    esac
    shift
  done

  # check required params
  [[ -z "${base64string}" ]] && { echo "{\"message\": \"failure\", \"response\": \"Missing required parameter: -b (base64 string)\" }" | jq; exit 2; }
  [[ -z "${blobURL}" ]] && { echo "{\"message\": \"failure\", \"response\": \"Missing required parameter: -u (blobURL string)\" }" | jq; exit 2;}

  return 0
}

args=("$@")
parse_params "${args[@]}"

#show_debug_info
filename=$(basename "$blobURL").png

{ 
  fileCreateResp=$(base64 -d <<< "$base64string" > "$filename")
} ||
{
  echo "{\"message\": \"failure\", \"response\": \"$fileCreateResp\"}" | jq
}

export pymage_file_path="$filename"; 

python << END

import time
import sys
import configparser
import os
from pathlib import Path
import boto3
import json

def process_text_analysis():
    try:
      start_time = None
      start_time = time.perf_counter() 
      png_file_path = os.environ["pymage_file_path"]
      png_file = open(png_file_path, 'rb')
      img_byte_arr = bytearray(png_file.read())
      
      client = boto3.client(
          get_config_value('aws_config','aws_service_name'),
          region_name=get_config_value('aws_config','aws_region_name'),
          aws_access_key_id=get_config_value('aws_config','aws_access_key_id'),
          aws_secret_access_key=get_config_value('aws_config','aws_secret_access_key')
      )
    
      response = client.detect_document_text(
          Document={'Bytes': img_byte_arr}
      )

      extracted_text = ''
      for item in response["Blocks"]:
          if item["BlockType"] == "LINE":
              extracted_text += item["Text"] + "\n"

      extracted_text = extracted_text.strip()
      extracted_text = extracted_text.replace('"', '')
      extracted_text = '\"' + extracted_text + '\"'

      elapsed_time = time.perf_counter() - start_time

      json_success_msg= {
              "message": "success",
              "extracted_text": extracted_text
              }

      json_data = '{"message": "success", ' \
          '"extracted_text": ' + extracted_text + '}'

      json_object = json.loads(json_data,strict=False)
      json_formatted_str = json.dumps(json_object, indent=2)

      print(json_formatted_str)
    except Exception as e:
        exc_type, exc_obj, exc_tb = sys.exc_info()
        json_data = '{"message": "error", ' \
          '"error": "' + str(e) + ' ' + str(exc_tb.tb_lineno) + '"}'
        print(json.dumps(json_data,indent=4))
        sys.exit()
    finally:
        sys.exit()

def get_config_value(config_section, config_item_name):
  path = Path(__file__)
  runtime_path = path.parent.absolute()
  config_path = os.path.join(runtime_path, "config.ini")
  config = configparser.ConfigParser()
  config.read(config_path)
  return config.get(config_section, config_item_name)

def main():
  process_text_analysis()

if __name__ == "__main__":
    main()

END


fileSpaceUsed=$(df -h / --output=pcent | tail -n 1 | cut -d'%' -f1)
if [ "$fileSpaceUsed" -lt 40 ]; then mv "$filename" ./images; else rm "$filename" ; fi

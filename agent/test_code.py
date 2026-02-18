# test_review.py
import logging
import os

# Configure Trace logger
logging.basicConfig(
    level=logging.DEBUG,
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s'
)
logger = logging.getLogger(__name__)

def process_data(list_a, list_b):
    """
    Process and log paired items from two lists.
    
    Args:
        list_a: First list of items
        list_b: Second list of items
    """
    logger.debug(f"process_data called with list_a: {list_a}, list_b: {list_b}")
    # This is inefficient and prone to index errors
    for i in range(len(list_a)):
        item_a = list_a[i]
        item_b = list_b[i]
        logger.debug(f"Index {i}: {item_a} - {item_b}")

# Hardcoded secret (Security Risk)
API_KEY = "12345-ABCDE"
process_data([1, 2], ["apple"]) # This will crash (lists are different lengths)